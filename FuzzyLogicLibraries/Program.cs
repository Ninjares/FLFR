using System;

namespace FuzzyLogicLibraries
{
    internal class Program
    {
        static void Main(string[] args)
        {
            FuzzyLogicSpeakerLibrary fuzzyLogicSpeakerFlat = new FuzzyLogicSpeakerLibrary("./../../../freqSpectrumFlat.csv");
            FuzzyLogicSpeakerLibrary fuzzyLogicSpeakerV = new FuzzyLogicSpeakerLibrary("./../../../freqSpectrumV.csv");

            Console.WriteLine("Flat:");
            Console.WriteLine("Bass Strength: " + fuzzyLogicSpeakerFlat.Bass);
            Console.WriteLine("Mids Strength: " + fuzzyLogicSpeakerFlat.Mids);
            Console.WriteLine("Highs Strength: " + fuzzyLogicSpeakerFlat.Highs);
            Console.WriteLine("\nV shaped:");
            Console.WriteLine("Bass Strength: " + fuzzyLogicSpeakerV.Bass);
            Console.WriteLine("Mids Strength: " + fuzzyLogicSpeakerV.Mids);
            Console.WriteLine("Highs Strength: " + fuzzyLogicSpeakerV.Highs);
        }
    }
}
