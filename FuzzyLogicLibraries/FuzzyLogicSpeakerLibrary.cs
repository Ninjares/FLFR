using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuzzyLogicLibraries
{
    struct SamplePoint
    {
        public double Hz;
        public double dB;
    }

    public class FuzzyLogicSpeakerLibrary
    {
        private double averageDb;
        private double MinDb;
        private double MaxDb;
        private double BellowAvaverageMultiplier;
        private double AboveAverageMultiplier;
        private List<SamplePoint> frequencyResponse;
        public FuzzyLogicSpeakerLibrary(string csvFrequencyData)
        {
            frequencyResponse = new List<SamplePoint>(); 
            using(TextFieldParser frequencyData = new TextFieldParser(csvFrequencyData))
            {
                frequencyData.SetDelimiters(";");
                frequencyData.ReadLine();
                while (!frequencyData.EndOfData)
                {
                    string[] fields = frequencyData.ReadFields();
                    frequencyResponse.Add(new SamplePoint { 
                        Hz = double.Parse(fields[0].Replace(',', '.')), 
                        dB = double.Parse(fields[1].Replace(',', '.')) 
                    });
                }
            }
            averageDb = frequencyResponse.Select(x => x.dB).Average();
            MinDb = frequencyResponse.Select(x => x.dB).Min();
            MaxDb = frequencyResponse.Select(x => x.dB).Max();
            BellowAvaverageMultiplier = 0.66;
            AboveAverageMultiplier = 0.34;


        }
        public double Bass { get => GetBassValue(); }
        public double Mids { get => GetMidsValue(); }
        public double Highs { get => GetHighsValue(); }

        private double GetBassValue()
        { 
            Func<double, double> multiplierfunc = (x) =>
            {
                if (x < 250) return 1 / (1 + Math.Pow(1.15, x - 200));
                else return 0;
            };

            List<double> MaxValues = new List<double>();
            List<double> calcvalues = new List<double>();
            foreach (SamplePoint sp in frequencyResponse.Where(x => x.Hz < 250)) 
            {
                double multiplier = multiplierfunc(sp.Hz);
                double truthvalueforHz;
                if (sp.dB >= MinDb && sp.dB <= averageDb)
                {
                    truthvalueforHz = ((sp.dB - MinDb) / (averageDb - MinDb)) 
                        * BellowAvaverageMultiplier * multiplier;
                }
                else if (sp.dB > averageDb && sp.dB <= MaxDb)
                {
                    truthvalueforHz = BellowAvaverageMultiplier +((sp.dB - averageDb) / (MaxDb - averageDb))
                        * AboveAverageMultiplier * multiplier;
                }
                else throw new ArgumentException("Db out of range: "+sp.dB);
                MaxValues.Add(1 * multiplier);
                calcvalues.Add(truthvalueforHz * multiplier);
            }
            return calcvalues.Sum() / MaxValues.Sum();
        }
        
        private double GetMidsValue()
        {
            Func<double, double> multiplierfunc = (x) =>
            {
                if (x < 150) return 0;
                else if (x > 150 && x <= 250) return 1 / (1 + Math.Pow(1.15, 200 - x));
                else if (x < 250 && x <= 1500) return 1;
                else if (x > 1500 && x <= 2500) return 1 / (1 + Math.Pow(1.015, x - 2000));
                else return 0;
            };
            List<double> MaxValues = new List<double>();
            List<double> calcvalues = new List<double>();
            foreach (SamplePoint sp in frequencyResponse.Where(x => x.Hz >= 150 && x.Hz <= 2500))
            {
                double multiplier = multiplierfunc(sp.Hz);
                double truthvalueforHz;
                if (sp.dB >= MinDb && sp.dB <= averageDb)
                {
                    truthvalueforHz = ((sp.dB - MinDb) / (averageDb - MinDb)) * BellowAvaverageMultiplier * multiplier;
                }
                else if (sp.dB > averageDb && sp.dB <= MaxDb)
                {
                    truthvalueforHz = BellowAvaverageMultiplier + ((sp.dB - averageDb) / (MaxDb - averageDb)) * AboveAverageMultiplier * multiplier;
                }
                else throw new ArgumentException("Db out of range: " + sp.dB);
                MaxValues.Add(1 * multiplier);
                calcvalues.Add(truthvalueforHz * multiplier);
            }
            return calcvalues.Sum() / MaxValues.Sum();
        }

        private double GetHighsValue()
        {
            Func<double, double> multiplierfunc = (x) =>
            {
                if (x < 1500) return 0;
                else return 1 / (1 + Math.Pow(1.015, 2000 - x));
                
            };
            List<double> MaxValues = new List<double>();
            List<double> calcvalues = new List<double>();
            foreach (SamplePoint sp in frequencyResponse.Where(x => x.Hz >= 1500 && x.Hz <= 20000))
            {
                double multiplier = multiplierfunc(sp.Hz);
                double truthvalueforHz;
                if (sp.dB >= MinDb && sp.dB <= averageDb)
                {
                    truthvalueforHz = ((sp.dB - MinDb) / (averageDb - MinDb)) * BellowAvaverageMultiplier * multiplier;
                }
                else if (sp.dB > averageDb && sp.dB <= MaxDb)
                {
                    truthvalueforHz = BellowAvaverageMultiplier + ((sp.dB - averageDb) / (MaxDb - averageDb)) * AboveAverageMultiplier * multiplier;
                }
                else throw new ArgumentException("Db out of range: " + sp.dB);
                MaxValues.Add(1 * multiplier);
                calcvalues.Add(truthvalueforHz * multiplier);
            }
            return calcvalues.Sum() / MaxValues.Sum();
        }

    }
}
