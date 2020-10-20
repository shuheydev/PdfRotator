using iTextSharp.text.pdf;
using System;
using System.IO;
using System.Linq;

namespace PdfTool
{
    public class Pdf
    {
        private readonly PdfReader _pdfReader;

        public Pdf(string filePath)
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException($"'{filePath}' not found.");
            }

            _pdfReader = new PdfReader(filePath);
        }

        public void Write(string outputPath)
        {
            using (FileStream fs = new FileStream(outputPath, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                PdfStamper stamper = new PdfStamper(_pdfReader, fs);
                stamper.Close();
            }
        }

        #region Rotate
        public int Rotate(int pageNumber, int rotateAngle)
        {
            //Make sure the pageNumber is in the valid range
            int pageCount = _pdfReader.NumberOfPages;
            if (pageNumber > pageCount || pageNumber < 1)
            {
                throw new ArgumentException($"Page number out of range. It must be (1-{pageCount})", "pageNumber");
            }

            //Make sure the rotateAngle is valid(0,90,180,270,-90,-180,-270).
            if (!IsAcceptableAngle(rotateAngle))
            {
                throw new ArgumentException($"Rotate degree is not acceptable. It must be ({string.Join(", ", _acceptableAngle)})", "rotateAngle");
            }

            //Rotate the page!
            var page = _pdfReader.GetPageN(pageNumber);
            page.Put(PdfName.Rotate, CalcRotatedObject(page, rotateAngle));

            //return angle after rotated.
            return page.GetAsNumber(PdfName.Rotate).IntValue;
        }

        private PdfNumber CalcRotatedObject(PdfDictionary page, int rotateAngle)
        {
            var rotate = page.GetAsNumber(PdfName.Rotate);

            return new PdfNumber(rotate == null ? rotateAngle : AdjustRotateAngle(rotate.IntValue + rotateAngle));
        }

        private int AdjustRotateAngle(int angle)
        {
            return angle % 360;
        }
        #endregion

        public int Count()
        {
            int pagesCount = _pdfReader.NumberOfPages;
            return pagesCount;
        }

        public int GetPageAngle(int pageNumber)
        {
            var page = _pdfReader.GetPageN(pageNumber);
            var angle = page.GetAsNumber(PdfName.Rotate);

            return angle is null ? 0 : angle.IntValue;
        }

        private static readonly int[] _acceptableAngle = { 0, 90, 180, 270, -0, -90, -180, -270 };

        public static bool IsAcceptableAngle(int angle)
        {
            if (_acceptableAngle.Contains(angle))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public static bool IsAcceptableAngle(string angle)
        {
            if (int.TryParse(angle, out int angleInt))
            {
                return IsAcceptableAngle(angleInt);
            }
            else
            {
                return false;
            }
        }
    }
}
