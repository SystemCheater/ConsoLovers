// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CommandInfo.cs" company="ConsoLovers">
//    Copyright (c) ConsoLovers  2015 - 2017
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ConsoLovers.ConsoleToolkit.CommandLineArguments
{
   using System;
   using System.Reflection;

   using JetBrains.Annotations;

   public class CommandInfo : ParameterInfo
   {
      #region Constructors and Destructors

      public CommandInfo([NotNull] PropertyInfo propertyInfo, [NotNull] CommandAttribute commandLineAttribute)
         : base(propertyInfo, commandLineAttribute)
      {
         ArgumentType = ComputeArgumentType();
      }

      private Type ComputeArgumentType()
      {
         var commandInterface = PropertyInfo.PropertyType.GetInterface(typeof(ICommand<>).FullName);
         if (commandInterface == null)
            return null;

         return commandInterface.GenericTypeArguments[0];
      }

      #endregion

      #region Public Properties

      public CommandAttribute Attribute => (CommandAttribute)CommandLineAttribute;

      /// <summary>Gets or sets the type of the argument.</summary>
      public Type ArgumentType { get; }

      #endregion
   }

   public class ArgumentInfo : ParameterInfo
   {
      #region Constructors and Destructors

      public ArgumentInfo([NotNull] PropertyInfo propertyInfo, [NotNull] ArgumentAttribute commandLineAttribute)
         : base(propertyInfo, commandLineAttribute)
      {
      }

      #endregion

      #region Public Properties

      public ArgumentAttribute Attribute => (ArgumentAttribute)CommandLineAttribute;

      #endregion
   }

   public class OptionInfo : ParameterInfo
   {
      #region Constructors and Destructors

      public OptionInfo([NotNull] PropertyInfo propertyInfo, [NotNull] OptionAttribute commandLineAttribute)
         : base(propertyInfo, commandLineAttribute)
      {
      }

      #endregion

      #region Public Properties

      public OptionAttribute Attribute => (OptionAttribute)CommandLineAttribute;

      #endregion
   }
}