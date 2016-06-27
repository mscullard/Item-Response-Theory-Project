using System;
using System.Collections.Generic;
using ExecutableIrt.Enumerations;
using IRT.Parameters;
using Microsoft.Office.Interop.Excel;

namespace ExecutableIrt.ExcelInteraction
{
    public class ItemInformationReader
    {
        private string ScaleColumn = "A1:A10000";
        private int RowOffset = 2;
        private int ScaleIndex = 0;
        private int ItemIndex = 1;
        private int ParameterAIndex = 2;
        private int ParameterBIndex = 3;
        private int ParameterCIndex = 4;
        private int ItemOrientationIndex = 5;

        public List<ItemInformation> GetItemInformation(Worksheet sheet)
        {
            int numItems = GetNumItems(sheet);

            List<ItemInformation> itemInformationList = new List<ItemInformation>();
            for (int i = 0; i < numItems; i++)
            {
                ItemInformation itemInformation = GetInformationForRow(i + RowOffset, sheet);
                itemInformationList.Add(itemInformation);
            }

            return itemInformationList;
        }

        private ItemInformation GetInformationForRow(int i, Worksheet sheet)
        {
            string rowRange = "A" + i + ":" + "F" + i;
            List<string> row = CellReader.GetRange(rowRange, sheet);

            ItemInformation itemInformation = new ItemInformation()
            {
                ItemName = row[ItemIndex],
                Orientation = (row[ItemOrientationIndex] == "N") ? Orientation.Normal : Orientation.Reversed,
                ParameterA = Convert.ToDouble(row[ParameterAIndex]),
                ParameterB = Convert.ToDouble(row[ParameterBIndex]),
                ParameterC = Convert.ToDouble(row[ParameterCIndex]),
                ScaleName = row[ScaleIndex]
            };

            return itemInformation;
        }

        private int GetNumItems(Worksheet sheet)
        {
            return CellReader.GetRange(ScaleColumn, sheet).Count;
        }
    }
}
