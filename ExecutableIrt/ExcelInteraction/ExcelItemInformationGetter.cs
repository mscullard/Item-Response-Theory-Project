using System;
using System.Collections.Generic;
using System.Data;
using IRT.Parameters;

namespace ExecutableIrt.ExcelInteraction
{
    public class ExcelItemInformationGetter
    {
        private const int ScaleNameColumnIndex = 0;
        private const int ItemNameColumnIndex = 1;
        private const int ParameterAColumnIndex = 2;
        private const int ParameterBColumnIndex = 3;
        private const int ParameterCColumnIndex = 4;
        private const int OrientationIndex = 5;

        private const int FirstParameterRow = 1;

        public List<ItemInformation> GetItemInformation(DataTable sheet)
        {
            List<ItemInformation> ItemInformationList = new List<ItemInformation>();

            for(int i = FirstParameterRow; i < sheet.Rows.Count; i++)
            {
                DataRow row = sheet.Rows[i];

                ItemInformation ItemInformation = new ItemInformation();
                ItemInformation.ScaleName = GetString(row, ScaleNameColumnIndex);
                ItemInformation.ItemName = GetString(row, ItemNameColumnIndex);
                ItemInformation.ParameterA = GetDouble(row, ParameterAColumnIndex);
                ItemInformation.ParameterB = GetDouble(row, ParameterBColumnIndex);
                ItemInformation.ParameterC = GetDouble(row, ParameterCColumnIndex);

                string orientationString = (string)row.ItemArray[OrientationIndex];

                ItemInformation.Orientation = orientationString == "N" ? Orientation.Normal : Orientation.Reversed;

                ItemInformationList.Add(ItemInformation);
            }

            return ItemInformationList;
        }

        private double GetDouble(DataRow row, int label)
        {
            string value = (string) row.ItemArray[label];

            return Convert.ToDouble(value);
        }

        private string GetString(DataRow row, int label)
        {
            return Convert.ToString(row.ItemArray[label]);
        }
    }
}
