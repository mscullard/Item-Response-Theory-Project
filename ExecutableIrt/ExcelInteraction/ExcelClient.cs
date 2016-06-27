using System;

namespace ExecutableIrt.ExcelInteraction
{
    public class ExcelClient
    {
        private Microsoft.Office.Interop.Excel.Application _app;

        public ExcelClient()
        {
           _app = new Microsoft.Office.Interop.Excel.Application();
        }

        public Microsoft.Office.Interop.Excel.Workbook GetSheets(string fileLocation)
        {
            Microsoft.Office.Interop.Excel.Workbook wb = _app.Workbooks.Open(fileLocation, Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                    Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);

            return wb;
        }

        public void CloseConnection()
        {
            _app.Quit();
        }

        public Microsoft.Office.Interop.Excel.Application GetApplication()
        {
            return _app;
        }
    }
}
