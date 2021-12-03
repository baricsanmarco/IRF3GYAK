using Mikroszimulacio.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Mikroszimulacio
{
    public partial class Form1 : Form
    {

        List<Person> Population = null;
        List<BirthProbability> BirthProbabilities = null;
        List<DeathProbability> DeathProbabilities = null;


        public Form1()
        {
            InitializeComponent();

            Population = GetPopulation(@"C:\Temp\nép-teszt.csv");
            BirthProbabilities = GetBirthProbabilities(@"C:\Temp\születés.csv");
            DeathProbabilities = GetDeathProbabilities(@"C:\Temp\halál.csv");
        }

        public List<Person> GetPopulation(string csvPath)
        {
            List<Person> population = new List<Person>();

            using (var sr = new StreamReader(csvPath, Encoding.Default))
            {
                while (!sr.EndOfStream)
                {
                    var line = sr.ReadLine().Split(';');
                    var p = new Person();
                    p.BirthYear = int.Parse(line[0]);
                    p.Gender = (Gender)Enum.Parse(typeof(Gender), line[1]);
                    p.NbrOfChildren = int.Parse(line[2]);
                    population.Add(p);
                }
            }

            return population;
        }
        public List<BirthProbability> GetBirthProbabilities(string csvPath)
        {
            List<BirthProbability> population = new List<BirthProbability>();

            using (var sr = new StreamReader(csvPath, Encoding.Default))
            {
                while (!sr.EndOfStream)
                {
                    var line = sr.ReadLine().Split(';');
                    var p = new BirthProbability();
                    p.Age = int.Parse(line[0]);
                    p.NbrOfChildren = int.Parse(line[1]);
                    p.P = double.Parse(line[2].Replace(",", "."));
                    population.Add(p);
                }
            }

            return population;
        }
        public List<DeathProbability> GetDeathProbabilities(string csvPath)
        {
            List<DeathProbability> population = new List<DeathProbability>();

            using (var sr = new StreamReader(csvPath, Encoding.Default))
            {
                while (!sr.EndOfStream)
                {
                    var line = sr.ReadLine().Split(';');
                    var p = new DeathProbability();
                    p.Gender = (Gender)Enum.Parse(typeof(Gender), line[0]);
                    p.Age = int.Parse(line[1]);
                    p.P = double.Parse(line[2].Replace(",","."));
                    population.Add(p);
                }
            }

            return population;
        }
    }
}
