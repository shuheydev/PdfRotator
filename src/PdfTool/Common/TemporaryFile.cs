using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace PdfTool.Common
{
    //Reference:
    // https://blog.jhashimoto.net/entry/20111211/1323639009
    // https://ufcpp.net/study/csharp/rm_disposable.html?sec=idisposable#idisposable

    /// <summary>
    /// Temporary file will be automatically deleted.
    /// </summary>
    public class TemporaryFile : IDisposable
    {
        private string fullName = Path.GetTempFileName();
        private bool disposedValue = false;

        public string FullName
        {
            get => fullName;
        }

        public void Dispose()
        {
            //GC前にプログラム的にリソースを破棄するので
            //管理,非管理リソース両方が破棄されるようにする
            Dispose(true);
            GC.SuppressFinalize(this);//破棄処理は完了しているのでGC不要の合図
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposedValue)
            {
                return;
            }

            if (disposing)
            {
                //管理リソースの破棄処理
            }

            //非管理リソースの破棄処理
            try
            {
                File.Delete(this.fullName);
            }
            catch
            {
                throw;
            }

            disposedValue = true;
        }

        ~TemporaryFile()
        {
            //GC時に実行されるデストラクタでは非管理リソースの削除のみ
            Dispose(false);
        }
    }
}
