﻿using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
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
                throw new FileNotFoundException($"'{filePath}' not found.");

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

        private static readonly int[] _acceptableDegree = { 0, 90, 180, 270, -0, -90, -180, -270 };

        public int Rotate(int pageNumber, int rotateDegree)
        {
            int pageCount = _pdfReader.NumberOfPages;
            if (pageNumber > _pdfReader.NumberOfPages)
                throw new ArgumentException($"Page number out of range. It must be (1-{pageCount})", "pageNumber");


            if (!_acceptableDegree.Contains(rotateDegree))
                throw new ArgumentException($"Rotate degree is not acceptable. It must be ({string.Join(", ", _acceptableDegree)})", "rotateDegree");

            var page = _pdfReader.GetPageN(pageNumber);
            var rotate = page.GetAsNumber(PdfName.Rotate);

            page.Put(PdfName.Rotate, new PdfNumber(rotate == null ? rotateDegree : (rotate.IntValue + rotateDegree) % 360));

            return page.GetAsNumber(PdfName.Rotate).IntValue;
        }

        public int Count()
        {
            int pagesCount = _pdfReader.NumberOfPages;
            return pagesCount;
        }

        public int GetPageRotate(int pageNumber)
        {
            var page = _pdfReader.GetPageN(pageNumber);
            var rotate = page.GetAsNumber(PdfName.Rotate);

            return rotate is null ? 0 : rotate.IntValue;
        }

        public static bool IsAcceptableDegree(int degree)
        {
            if (_acceptableDegree.Contains(degree))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool IsAcceptableDegree(string degree)
        {
            if (int.TryParse(degree, out int degreeInt))
            {
                return IsAcceptableDegree(degreeInt);
            }
            else
            {
                return false;
            }
        }
    }
}
