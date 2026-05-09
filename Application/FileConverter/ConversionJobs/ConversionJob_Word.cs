// <copyright file="ConversionJob_Word.cs" company="AAllard">License: http://www.gnu.org/licenses/gpl.html GPL version 3.</copyright>

namespace FileConverter.ConversionJobs
{
    using System;
    using System.IO;
    using System.Runtime.InteropServices;
    using System.Threading;
    using System.Threading.Tasks;

    using FileConverter.Diagnostics;

    using Word = NetOffice.WordApi;

    public class ConversionJob_Word : ConversionJob_Office
    {
        private const int OfficeRetryCount = 20;
        private const int OfficeRetryDelayInMilliseconds = 250;
        private const int RpcCallRejected = unchecked((int)0x80010001);
        private const int RpcServerCallRetryLater = unchecked((int)0x8001010A);
        private const int VbaIgnore = unchecked((int)0x800AC472);

        private Word.Document document;
        private Word.Application application;

        private string intermediateFilePath = string.Empty;
        private ConversionJob pdf2ImageConversionJob = null;

        public ConversionJob_Word() : base()
        {
        }

        public ConversionJob_Word(ConversionPreset conversionPreset, string inputFilePath) : base(conversionPreset, inputFilePath)
        {
        }

        protected override ApplicationName Application => ApplicationName.Word;

        protected override bool IsCancelable() => false;

        protected override int GetOutputFilesCount()
        {
            if (this.ConversionPreset.OutputType == OutputType.Pdf)
            {
                return 1;
            }

            try
            {
                if (!this.TryLoadDocumentIfNecessary())
                {
                    return 1;
                }

                int pagesCount = this.ExecuteWordOperation(() => this.document.ComputeStatistics(Word.Enums.WdStatistic.wdStatisticPages));

                return pagesCount;
            }
            catch (Exception exception)
            {
                Debug.Log(exception.ToString());

                return 1;
            }
            finally
            {
                this.CloseDocumentIfNeeded();
                this.ReleaseOfficeApplicationInstanceIfNeeded();
            }
        }

        protected override void Initialize()
        {
            base.Initialize();

            if (this.State == ConversionState.Failed)
            {
                return;
            }

            if (this.ConversionPreset == null)
            {
                throw new System.Exception("The conversion preset must be valid.");
            }

            // Initialize converters.
            if (this.ConversionPreset.OutputType == OutputType.Pdf)
            {
                this.intermediateFilePath = this.OutputFilePath;
            }
            else
            {
                // Generate intermediate file path.
                string fileName = Path.GetFileNameWithoutExtension(this.InputFilePath);
                string tempPath = Path.GetTempPath();
                this.intermediateFilePath = PathHelpers.GenerateUniquePath(tempPath + fileName + ".pdf");

                ConversionPreset intermediatePreset = new ConversionPreset("Pdf to image", this.ConversionPreset, "pdf");
                this.pdf2ImageConversionJob = ConversionJobFactory.Create(intermediatePreset, this.intermediateFilePath);
                this.pdf2ImageConversionJob.PrepareConversion(this.OutputFilePaths);
            }
        }

        protected override void Convert()
        {
            if (this.ConversionPreset == null)
            {
                throw new System.Exception("The conversion preset must be valid.");
            }

            this.UserState = Properties.Resources.ConversionStateReadDocument;

            try
            {
                if (!this.TryLoadDocumentIfNecessary())
                {
                    this.ConversionFailed(Properties.Resources.ErrorUnableToUseMicrosoftOffice);
                    return;
                }

                // Make this document the active document.
                this.ExecuteWordOperation(() => this.document.Activate());

                this.UserState = Properties.Resources.ConversionStateConversion;

                Debug.Log("Convert word document to pdf.");
                // this.document.ExportAsFixedFormat(this.intermediateFilePath, Word.WdExportFormat.wdExportFormatPDF);
                this.ExecuteWordOperation(() => this.document.ExportAsFixedFormat(this.intermediateFilePath,
                    Word.Enums.WdExportFormat.wdExportFormatPDF,
                    false,
                    Word.Enums.WdExportOptimizeFor.wdExportOptimizeForPrint,
                    Word.Enums.WdExportRange.wdExportAllDocument,
                    1, 1,
                    Word.Enums.WdExportItem.wdExportDocumentContent,
                    true,
                    true,
                    Word.Enums.WdExportCreateBookmarks.wdExportCreateHeadingBookmarks,
                    true));
            }
            catch (Exception exception)
            {
                Debug.Log(exception.ToString());
                this.ConversionFailed(Properties.Resources.ErrorUnableToUseMicrosoftOffice);
                return;
            }
            finally
            {
                this.CloseDocumentIfNeeded();
                this.ReleaseOfficeApplicationInstanceIfNeeded();
            }
            
            if (this.pdf2ImageConversionJob != null)
            {
                if (!System.IO.File.Exists(this.intermediateFilePath))
                {
                    this.ConversionFailed(Properties.Resources.ErrorCantFindOutputFiles);
                    return;
                }

                Task updateProgress = this.UpdateProgress();

                Debug.Log("Convert pdf to images.");

                this.pdf2ImageConversionJob.StartConversion();

                if (this.pdf2ImageConversionJob.State != ConversionState.Done)
                {
                    this.ConversionFailed(this.pdf2ImageConversionJob.ErrorMessage);
                    return;
                }

                if (!string.IsNullOrEmpty(this.intermediateFilePath))
                {
                    Debug.Log($"Delete intermediate file {this.intermediateFilePath}.");

                    File.Delete(this.intermediateFilePath);
                }

                updateProgress.Wait();
            }
        }

        protected override void InitializeOfficeApplicationInstanceIfNecessary()
        {
            if (this.application != null)
            {
                return;
            }

            // Initialize word application.
            Debug.Log("Instantiate word application via interop.");
            this.application = new Word.Application
            {
                Visible = false
            };

            this.ExecuteWordOperation(() =>
            {
                this.application.DisplayAlerts = Word.Enums.WdAlertLevel.wdAlertsNone;
            });
        }

        protected override void ReleaseOfficeApplicationInstanceIfNeeded()
        {
            if (this.application == null)
            {
                return;
            }

            try
            {
                Diagnostics.Debug.Log("Quit word application via interop.");
                this.ExecuteWordOperation(() => this.application.Quit());
            }
            catch (Exception exception)
            {
                Debug.Log(exception.ToString());
            }
            finally
            {
                this.application.Dispose();
                this.application = null;
            }
        }

        private async Task UpdateProgress()
        {
            while (this.pdf2ImageConversionJob.State != ConversionState.Done &&
                   this.pdf2ImageConversionJob.State != ConversionState.Failed)
            {
                if (this.pdf2ImageConversionJob != null && this.pdf2ImageConversionJob.State == ConversionState.InProgress)
                {
                    this.Progress = this.pdf2ImageConversionJob.Progress;
                }

                if (this.pdf2ImageConversionJob != null && this.pdf2ImageConversionJob.State == ConversionState.InProgress)
                {
                    this.Progress = this.pdf2ImageConversionJob.Progress;
                    this.UserState = this.pdf2ImageConversionJob.UserState;
                }

                await Task.Delay(40);
            }
        }

        private bool TryLoadDocumentIfNecessary()
        {
            try
            {
                this.InitializeOfficeApplicationInstanceIfNecessary();
            }
            catch (Exception exception)
            {
                Debug.Log(exception.ToString());
                Debug.Log("Failed to initialize office application.");
            }

            if (this.application == null)
            {
                return false;
            }

            if (this.document == null)
            {
                Debug.Log($"Load word document '{this.InputFilePath}'.");

                this.document = this.ExecuteWordOperation(() => this.application.Documents.Open(this.InputFilePath, System.Reflection.Missing.Value, true, false));
            }

            return this.document != null;
        }

        private void CloseDocumentIfNeeded()
        {
            if (this.document == null)
            {
                return;
            }

            try
            {
                Debug.Log($"Close word document '{this.InputFilePath}'.");
                this.ExecuteWordOperation(() => this.document.Close(Word.Enums.WdSaveOptions.wdDoNotSaveChanges));
            }
            catch (Exception exception)
            {
                Debug.Log(exception.ToString());
            }
            finally
            {
                this.document.Dispose();
                this.document = null;
            }
        }

        private void ExecuteWordOperation(Action action)
        {
            this.ExecuteWordOperation(
                () =>
                {
                    action();
                    return true;
                });
        }

        private T ExecuteWordOperation<T>(Func<T> action)
        {
            for (int attempt = 0; attempt < OfficeRetryCount; attempt++)
            {
                try
                {
                    return action();
                }
                catch (COMException exception)
                {
                    if (!ConversionJob_Word.IsRetryableOfficeException(exception) || attempt == OfficeRetryCount - 1)
                    {
                        throw;
                    }

                    Debug.Log(exception.ToString());
                    Thread.Sleep(OfficeRetryDelayInMilliseconds);
                }
            }

            return action();
        }

        private static bool IsRetryableOfficeException(COMException exception)
        {
            return exception.ErrorCode == RpcCallRejected ||
                   exception.ErrorCode == RpcServerCallRetryLater ||
                   exception.ErrorCode == VbaIgnore;
        }
    }
}
