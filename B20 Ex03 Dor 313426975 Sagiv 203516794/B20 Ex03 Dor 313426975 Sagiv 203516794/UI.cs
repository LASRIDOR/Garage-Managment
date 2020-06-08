﻿using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using Ex03.GarageLogic;

namespace B20_Ex03_Dor_313426975_Sagiv_203516794
{
    public class UI
    {
        private static readonly int sr_PageWidth = 70;
        private static readonly int sr_StartOfEnumUserChoice = 0;

        public enum eDisplayOption
        {
            AllVehicles = 0,
            SpecificStatus,
        }
        public enum eUserChoice
        {
            InsertNewCar = 0,
            DisplayDriverLicenceNumberWithFilterByStatusOfFix,
            ChangeStatusOfFix,
            BlowWheelsToMaximum,
            ChargeVehicle,
            DisplayFullDetailsOfVehicleByLicenceNumber,
            Exit
        }

        public int PrintAllEnumValuesGetUserChoice<T>(T i_UserChoice, string i_MsgInSign)
        {
            int userChoice;

            printSign(string.Format("{0}", i_MsgInSign));
            userChoice = printEnumOptionsAndGetEnumChoiceFromUser(typeof(T));

            return userChoice;
        }

        private int printEnumOptionsAndGetEnumChoiceFromUser(Type i_TypeOFEnum)
        {
            int UserInput;

            printEnumMenuToUser(i_TypeOFEnum);
            UserInput = getInputOfEnumFromUser(i_TypeOFEnum);

            return UserInput;
        }

        private void printEnumMenuToUser(Type i_EnumType)
        {
            int currentIndex = sr_StartOfEnumUserChoice;
            string[] listOfChoices = Enum.GetNames(i_EnumType);
            StringBuilder enumOptionToPrint = new StringBuilder();

            foreach (string currName in listOfChoices)
            {
                string seperatedEnumName = seperateEnumName(currName);
                enumOptionToPrint.AppendFormat("To {0} Press {1}{2}", seperatedEnumName, currentIndex++, Environment.NewLine);
            }

            enumOptionToPrint.Append("Your Choice: ");

            Console.WriteLine(enumOptionToPrint.ToString());
        }

        private string seperateEnumName(string io_EnumName)
        {
            StringBuilder result = new StringBuilder();
            char previous = io_EnumName[0];

            for (int i = 0; i < io_EnumName.Length; i++)
            {
                if (io_EnumName[i] >= 'A' && io_EnumName[i] <= 'Z' && (previous >= 'a' && previous <= 'z'))
                {
                    result.Append(' ');
                }

                result.Append(io_EnumName[i]);
            }

            return result.ToString();
        }

        private int getInputOfEnumFromUser(Type i_EnumType)
        {
            int numberEnteredByUser = -1;
            string UserBabyInput;
            bool v_validInput;

            do
            {
                try
                {
                    UserBabyInput = Console.ReadLine();
                    numberEnteredByUser = int.Parse(UserBabyInput);
                    v_validInput = Enum.IsDefined(i_EnumType, numberEnteredByUser);
                }
                catch(FormatException)
                {
                    v_validInput = false;
                    Console.WriteLine("Input Is Invalid Try Again (pay attention you asked to enter a number)");
                }


            } while (v_validInput == false);

            return numberEnteredByUser;
        }

        public void PrintSignToUser(string i_Sign)
        {
            printSign(i_Sign);
        }

        public string GetStringWIthoutConditionFromUser(string i_DetailToGet)
        {
            string userInput;
            bool v_ValidInput;

            Console.Write(String.Format("Please Enter Your {0}: ", i_DetailToGet));

            do
            {
                userInput = Console.ReadLine();
                Console.Write(Environment.NewLine);
                v_ValidInput = userInput != null && userInput != " ";
            }
            while (v_ValidInput == false);

            return userInput;
        }

        public float getFloatWithoutAnyCondition(string i_DetailToGet)
        {
            string userInput;
            float userFloatChoice;
            bool v_ValidInput;

            Console.Write(String.Format("Please Enter Your {0}: ", i_DetailToGet));

            do
            {
                userInput = Console.ReadLine();
                Console.Write(Environment.NewLine);
                v_ValidInput = float.TryParse(userInput, out userFloatChoice);
            }
            while (v_ValidInput == false);

            return userFloatChoice;
        }

        public void PrintLicensePlatesWithStatusFilterIfNeeded(int i_UserChoice, Dictionary<string, VehicleRegistrationForm> i_TreatmentList, bool i_DisplayAll)
        {
            VehicleRegistrationForm.eStatusOfFix wantedStatus = (VehicleRegistrationForm.eStatusOfFix)i_UserChoice;

            foreach (VehicleRegistrationForm current in i_TreatmentList.Values)
            {
                if (current.Status == wantedStatus || i_DisplayAll)
                {
                    Console.WriteLine(current.Vehicle.LicenceNumber);
                }
            }
        }

        public void GetVehicleLicenseNumber(GarageManagment i_Garage, out string i_LicensePlate)
        {
            do
            {
                Console.WriteLine("Insert vehicle license Number");
                i_LicensePlate = Console.ReadLine();
            }
            while (string.IsNullOrEmpty(i_LicensePlate));
            i_Garage.IsVehicleExists(i_LicensePlate);
        }

        public ChargingVehicleDetails fillChargingVehicleForm(GarageManagment io_Garage)
        {
            string licenseNumber;
            float quantityToAdd;
            EnergySource.eTypeOfEnergySource typeOfEnergyChoice = new EnergySource.eTypeOfEnergySource();
            Fuel.eFuelType? typeOfFuelChoice = new Fuel.eFuelType();

            GetVehicleLicenseNumber(io_Garage, out licenseNumber);
            quantityToAdd = getFloatWithoutAnyCondition("Quantity To Add");
            typeOfEnergyChoice = (EnergySource.eTypeOfEnergySource)PrintAllEnumValuesGetUserChoice(typeOfEnergyChoice, "Fuel Type Choosing"); ;

            if (typeOfEnergyChoice == EnergySource.eTypeOfEnergySource.Fuel)
            {
                typeOfFuelChoice = (Fuel.eFuelType)PrintAllEnumValuesGetUserChoice(typeOfFuelChoice, "Fuel Type Choosing");
            }
            else
            {
                typeOfFuelChoice = null;
            }

            return new ChargingVehicleDetails(licenseNumber, quantityToAdd, typeOfEnergyChoice, typeOfFuelChoice.Value);
        }

        private void printSign(string i_Title)
        {
            string firstAndLastLineOfRectangle = new string('-', sr_PageWidth);
            string spacesWithPlaceToEdgesOfRectangle = new string(' ', sr_PageWidth - 2);

            string oneSideOfSpacesWithPlaceToEdgesOfRectangleAndTitle =
                new string(' ', (sr_PageWidth - 1 - i_Title.Length) / 2);

            string middleOfRectangle = string.Format("|{0}|", spacesWithPlaceToEdgesOfRectangle);

            string middleOfRectangleTitleLine =
                string.Format("|{0}{1}{0}|", oneSideOfSpacesWithPlaceToEdgesOfRectangleAndTitle, i_Title);

            System.Console.WriteLine(firstAndLastLineOfRectangle);
            System.Console.WriteLine(middleOfRectangle);
            System.Console.WriteLine(middleOfRectangleTitleLine);
            System.Console.WriteLine(middleOfRectangle);
            System.Console.WriteLine(firstAndLastLineOfRectangle);
        }

        public void PrintInvalidErrorWithSpecificError(string i_ErrorMsg)
        {
            Console.WriteLine(String.Format("Invalid Input Of Choice Try Again ({})", i_ErrorMsg));
        }

        public void PrintNatural(string i_Msg)
        {
            Console.WriteLine(i_Msg);
        }
    }
}
