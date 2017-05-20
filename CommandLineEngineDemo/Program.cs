﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Program.cs" company="ConsoLovers">
//    Copyright (c) ConsoLovers  2015 - 2017
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace CommandLineEngineDemo
{
   using ConsoLovers.ConsoleToolkit;
   using ConsoLovers.ConsoleToolkit.CommandLineArguments;

   class Program : ConsoleApplicationWith<Commands>
   {
      #region Public Methods and Operators


      public override void RunWith(Commands arguments)
      {
         if (arguments.Wait)
         {
            Console.WriteLine("Waiting for another key to be pressed");
            Console.ReadLine();
         }
      }

      public override bool RunWithCommand(ICommand command)
      {
         base.RunWithCommand(command);
         return true;
      }

      #endregion

      #region Methods

      static void Main(string[] args)
      {
         ConsoleApplicationManager.RunThis(args);
         Console.ReadLine();
      }

      #endregion
   }
}