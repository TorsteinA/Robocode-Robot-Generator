﻿using System;

namespace GeneticAlgorithmForStrings {
    internal class Algorithm {

        /* GA parameters */
		//Tweak. Algorithm tries to create this solution
//        internal static readonly string Solution = "Got my Genetic Algorithm to work with sentences now! \nWop Wop Wop Wop!\n:D :D :D\n:D :D\n:D\n:D :D\n:D :D :D\n\nDen klarer å skrive ord som \"Pokémon\" også! \\m/";

		//Tweak. Too low and it breaks, too high, and each generation will take forever.
	    internal static readonly int PopulationSize = 500;
		//Tweak. Too high creates random gibberish, too low never finds the solution.
        private const double MutationRate = 0.025;
		//Letters that algorithm can make genes with
		internal static readonly string AllowedLetters = "agct";  //abcdefghijklmnopqrstuvwxyzæøåèéêëàáâäíìîïùúûüABCDEFGHIJKLMNOPQRSTUVWXYZÆØÅÈÉÊËÀÁÂÄÍÌÎÏÙÚÛÜ0123456789 ,.:!?¨^~'*;-_/()=&%¤¤#|§\"\\\n\t";	// agct";
        //DNA length
        internal static int DefaultGeneLength = 1000;

        //Crossover tournament population size
        private const int TournamentSize = 40;
        //How much DNA to take from each parent. Should stay at 0.5
        private const double UniformRate = 0.5;
        //Keep copy of best individual next generation, or just random?
        private const bool Elitism = true;

        private static readonly Random Rnd = new Random();

		/*
	    internal static void RunGeneticAlgorithm()
	    {
			FitnessCalc.SetSolution(Solution);
		    var myPop = new Population(PopulationSize, true);
		    var generationCount = 0;

		    while (myPop.GetFittest().GetFitness() < FitnessCalc.GetMaxFitness()) {
			    generationCount++;

			    Console.WriteLine();
			    Console.ForegroundColor = ConsoleColor.DarkYellow;
			    Console.Write("Generation: ");
			    Console.ForegroundColor = ConsoleColor.White;
			    Console.Write(generationCount);
			    Console.ForegroundColor = ConsoleColor.DarkYellow;
			    Console.Write(", \t Fittest score: ");
			    Console.ForegroundColor = ConsoleColor.White;
			    Console.Write(myPop.GetFittest().GetFitness() + "/" + FitnessCalc.GetMaxFitness());
			    Console.ForegroundColor = ConsoleColor.DarkYellow;
			    Console.WriteLine(", \t Genes of fittest: ");
			    Console.ForegroundColor = ConsoleColor.White;
			    Console.WriteLine(myPop.GetFittest().ToString());

			    myPop = EvolvePopulation(myPop);
		    }

		    Console.WriteLine();
		    Console.ForegroundColor = ConsoleColor.Green;
		    Console.WriteLine("Solution Found!");
		    Console.Write("Generation: ");
		    Console.ForegroundColor = ConsoleColor.Yellow;
		    Console.WriteLine(generationCount);
		    Console.WriteLine();
		    Console.ForegroundColor = ConsoleColor.Red;
		    Console.WriteLine("MyPop Genes:    ");
		    Console.ForegroundColor = ConsoleColor.White;
		    Console.Write(myPop.GetFittest());
		    Console.ForegroundColor = ConsoleColor.Red;
		    Console.WriteLine();
		    Console.WriteLine();
		    Console.WriteLine("Solution Genes: ");
		    Console.ForegroundColor = ConsoleColor.White;
		    Console.Write(Solution);
		    Console.WriteLine();
		    Console.WriteLine();
		    Console.WriteLine();
		}
		*/

		/// <summary>
		/// Evolve a population
		/// </summary>
		/// <param name="pop">Population to evolve</param>
		/// <returns>Evolved population</returns>
		internal static Population EvolvePopulation(Population pop)
        {
            var newPopulation = new Population(pop.Size(), false);

            // Keep our best individual
            if (Elitism) 
                newPopulation.SaveIndividual(0, pop.GetFittest());
            
            // Crossover population
            const int elitismOffset = Elitism ? 1 : 0;

            // Loop over the population size and create new individuals with crossover
            for (var i = elitismOffset; i < pop.Size(); i++) {
                var indiv1 = TournamentSelection(pop);
                var indiv2 = TournamentSelection(pop);
                var newIndiv = Crossover(indiv1, indiv2);
                newPopulation.SaveIndividual(i, newIndiv);
            }

            // Mutate population
            for (var i = elitismOffset; i < newPopulation.Size(); i++)
                Mutate(newPopulation.GetIndividual(i));
            
            return newPopulation;
        }

		/// <summary>
		/// Crossover individuals
		/// </summary>
		/// <param name="indiv1">Parent individual 1</param>
		/// <param name="indiv2">Parent individual 2</param>
		/// <returns>Child individual</returns>
		private static Individual Crossover(Individual indiv1, Individual indiv2)
        {
            var newSol = new Individual();
            // Loop through genes
            for (var i = 0; i < indiv1.Size(); i++) {
                // Crossover
                newSol.SetGene(i, Rnd.NextDouble() <= UniformRate ? indiv1.GetGene(i) : indiv2.GetGene(i));
            }
            return newSol;
        }

		/// <summary>
		/// Mutate an individual
		/// </summary>
		/// <param name="indiv">Individual to mutate</param>
		private static void Mutate(Individual indiv)
        {
            // Loop through genes
            for (var i = 0; i < indiv.Size(); i++) {
                if (!(Rnd.NextDouble() <= MutationRate)) continue;
                // Create random gene
                var gene = AllowedLetters[Rnd.Next(AllowedLetters.Length)];
                indiv.SetGene(i, gene);
            }
        }

		/// <summary>
		/// Select individual for crossover
		/// </summary>
		/// <param name="pop">Population to select from</param>
		/// <returns>Fittest individual from tournament</returns>
		private static Individual TournamentSelection(Population pop)
        {
            // Create a tournament population
            var tournament = new Population(TournamentSize, false);
            // For each place in the tournament get a random individual
            for (var i = 0; i < TournamentSize; i++) {
                var randomId = Rnd.Next(pop.Size());
                tournament.SaveIndividual(i, pop.GetIndividual(randomId));
            }
            // Get the fittest
            var fittest = tournament.GetFittest();
            return fittest;
        }
    }
}