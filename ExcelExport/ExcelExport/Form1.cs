using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;
using System.Reflection;

namespace ExcelExport
{
    
    public partial class Form1 : Form
    {
        private int _million = 1000000;
        RealEstateEntities context = new RealEstateEntities();
        List<Flat> lakasok;
        Excel.Application xlApp;
        Excel.Workbook xlWB;
        Excel.Worksheet xlSheet;

        public Form1()
        {
            InitializeComponent();
            LoadData();
            dataGridView1.DataSource = lakasok;
            CreateExcel();
            CreateTable();
            FormatTable();
        }
        public void LoadData()
        {
            lakasok = context.Flats.ToList();
        }

        public void CreateExcel()
        {
            try
            {
                xlApp = new Excel.Application();
                xlWB = xlApp.Workbooks.Add(Missing.Value); //üres dokumentumot hozunk létre
                xlSheet = xlWB.ActiveSheet; //alapértelmezetten van 1 munkalap a munkafüzeten


                xlApp.Visible = true;
                xlApp.UserControl = true;
            }
            catch (Exception ex) // fontos a sorrend, először a munkafüzet, utána az app stb.
            {
                string hiba = string.Format("Error: {0}\nLine: {1}", ex.Message, ex.Source);
                MessageBox.Show(hiba, "Error");

                xlWB.Close(false, Type.Missing, Type.Missing); 
                xlApp.Quit();
                xlWB = null;
                xlApp = null; //Visual Studionak a memoriáját is törölni szükséges
            }
        }
        private void CreateTable()
        {
            //tömbbe kiírjük a fejléc adatokat
            string[] headers = new string[]
            {
                 "Kód",
                 "Eladó",
                 "Oldal",
                 "Kerület",
                 "Lift",
                 "Szobák száma",
                 "Alapterület (m2)",
                 "Ár (mFt)",
                 "Négyzetméter ár (Ft/m2)"
            };
            for (int i = 0; i < headers.Length; i++)
            {
                xlSheet.Cells[1, i + 1] = headers[i]; //+1 oka, hogy az egyik 0-ról a másik 1-ről van indexelve, mivel az excel 1-től indexel
            }

            //2 dimenziós tömb létrehozása, mivel az excel range-eket kezel hatékonyan, nem pedig cellákat
            //Annyi sor ahány lakás, annyi oszlop ahány fejléc cím

            object[,] values = new object[lakasok.Count,headers.Length];

            int counter = 0;
            int floorColumn = 6;
            foreach (var lakas in lakasok)
            {
                values[counter, 0] = lakas.Code;
                values[counter, 1] = lakas.Vendor;
                values[counter, 2] = lakas.Side;
                values[counter, 3] = lakas.District;
                values[counter, 4] = lakas.Elevator;//igaz-hamis helyett a van-nincs páros megjelenítése
                if (lakas.Elevator == true)
                {
                    values[counter, 4] = "Van";
                }
                else 
                {
                    values[counter, 4] = "Nincs";
                }
                values[counter, 5] = lakas.NumberOfRooms;
                values[counter, 6] = lakas.FloorArea;
                values[counter, 7] = lakas.Price;
                values[counter, 8] = string.Format("={0}/{1}*{2}",
                    "H" + (counter +2).ToString(),
                    GetCell(counter +2, floorColumn +1),
                    _million.ToString());
                counter++;

            }
           var range = xlSheet.get_Range(
                GetCell(2, 1),
                GetCell(1 + values.GetLength(0), values.GetLength(1))
                );
            range.Value2 = values;
        }
        //nem kell magunktól írni
        private string GetCell(int x, int y)
        {
            string ExcelCoordinate = "";
            int dividend = y;
            int modulo;

            while (dividend > 0)
            {
                modulo = (dividend - 1) % 26;
                ExcelCoordinate = Convert.ToChar(65 + modulo).ToString() + ExcelCoordinate;
                dividend = (int)((dividend - modulo) / 26);
            }
            ExcelCoordinate += x.ToString();

            return ExcelCoordinate;
        }

        private void FormatTable()
        {
            Excel.Range headerRange = xlSheet.get_Range(GetCell(1, 1), GetCell(1, headers.Length));
            headerRange.Font.Bold = true;
            headerRange.VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;
            headerRange.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
            headerRange.EntireColumn.AutoFit();
            headerRange.RowHeight = 40;
            headerRange.Interior.Color = Color.LightBlue;
            headerRange.BorderAround2(Excel.XlLineStyle.xlContinuous, Excel.XlBorderWeight.xlThick);
        }
    }
}
