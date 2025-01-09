using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using SwissWorkSafe.Models.Core;

namespace SwissWorkSafeTests
{
    [TestClass]
    public class TerminationTests
    {
        #region Constructor Tests

        [TestMethod]
        public void Termination_ValidParameters_ShouldCreateInstance()
        {
            // Arrange
            DateTime startDate = new DateTime(2020, 1, 1);
            DateTime terminationDate = new DateTime(2023, 1, 1);
            int sickDays = 10;
            string absenceReason = "krankheit";
            DateTime? reasonStartDate = null;
            int yearsOfService = 3;

            // Act
            var termination = new Termination(startDate, terminationDate, sickDays, absenceReason, reasonStartDate, yearsOfService);

            // Assert
            Assert.AreEqual(startDate, termination.StartDate, "StartDate wurde nicht korrekt gesetzt.");
            Assert.AreEqual(terminationDate, termination.TerminationDate, "TerminationDate wurde nicht korrekt gesetzt.");
            Assert.AreEqual(sickDays, termination.SickDays, "SickDays wurden nicht korrekt gesetzt.");
            Assert.AreEqual(absenceReason.ToLower(), termination.AbsenceReason, "AbsenceReason wurde nicht korrekt gesetzt.");
            Assert.AreEqual(reasonStartDate, termination.ReasonStartDate, "ReasonStartDate wurde nicht korrekt gesetzt.");
            Assert.AreEqual(yearsOfService, termination.YearsOfService, "YearsOfService wurden nicht korrekt gesetzt.");
        }

        [TestMethod]
        public void Termination_StartDateAfterTerminationDate_ShouldThrowArgumentException()
        {
            // Arrange
            DateTime startDate = new DateTime(2024, 1, 1);
            DateTime terminationDate = new DateTime(2023, 1, 1);
            int sickDays = 5;
            string absenceReason = "krankheit";
            DateTime? reasonStartDate = null;
            int yearsOfService = 3;

            // Act & Assert
            var ex = Assert.ThrowsException<ArgumentException>(() =>
                new Termination(startDate, terminationDate, sickDays, absenceReason, reasonStartDate, yearsOfService)
            );

            StringAssert.Contains(ex.Message, "The start date cannot be after the termination date.");
        }

        [TestMethod]
        public void Termination_NegativeSickDays_ShouldThrowArgumentOutOfRangeException()
        {
            // Arrange
            DateTime startDate = new DateTime(2020, 1, 1);
            DateTime terminationDate = new DateTime(2023, 1, 1);
            int sickDays = -1;
            string absenceReason = "krankheit";
            DateTime? reasonStartDate = null;
            int yearsOfService = 3;

            // Act & Assert
            var ex = Assert.ThrowsException<ArgumentOutOfRangeException>(() =>
                new Termination(startDate, terminationDate, sickDays, absenceReason, reasonStartDate, yearsOfService)
            );

            StringAssert.Contains(ex.Message, "Sick days cannot be negative.");
        }

        [TestMethod]
        public void Termination_EmptyAbsenceReason_ShouldThrowArgumentException()
        {
            // Arrange
            DateTime startDate = new DateTime(2020, 1, 1);
            DateTime terminationDate = new DateTime(2023, 1, 1);
            int sickDays = 5;
            string absenceReason = "  ";
            DateTime? reasonStartDate = null;
            int yearsOfService = 3;

            // Act & Assert
            var ex = Assert.ThrowsException<ArgumentException>(() =>
                new Termination(startDate, terminationDate, sickDays, absenceReason, reasonStartDate, yearsOfService)
            );

            StringAssert.Contains(ex.Message, "The absence reason cannot be empty.");
        }

        [TestMethod]
        public void Termination_InvalidAbsenceReason_ShouldThrowArgumentException()
        {
            // Arrange
            DateTime startDate = new DateTime(2020, 1, 1);
            DateTime terminationDate = new DateTime(2023, 1, 1);
            int sickDays = 5;
            string absenceReason = "invalid_reason";
            DateTime? reasonStartDate = null;
            int yearsOfService = 3;

            // Act & Assert
            var ex = Assert.ThrowsException<ArgumentException>(() =>
                new Termination(startDate, terminationDate, sickDays, absenceReason, reasonStartDate, yearsOfService)
            );

            StringAssert.Contains(ex.Message, $"The absence reason '{absenceReason}' is not recognized. Please choose a valid reason from the allowed list.");
        }

        [TestMethod]
        public void Termination_MilitaryServiceWithoutReasonStartDate_ShouldThrowArgumentException()
        {
            // Arrange
            DateTime startDate = new DateTime(2020, 1, 1);
            DateTime terminationDate = new DateTime(2023, 1, 1);
            int sickDays = 5;
            string absenceReason = "militärdienst";
            DateTime? reasonStartDate = null;
            int yearsOfService = 3;

            // Act & Assert
            var ex = Assert.ThrowsException<ArgumentException>(() =>
                new Termination(startDate, terminationDate, sickDays, absenceReason, reasonStartDate, yearsOfService)
            );

            StringAssert.Contains(ex.Message, "A reason start date is required for the absence reason 'militärdienst'.");
        }

        [TestMethod]
        public void Termination_BetreuungsurlaubWithoutReasonStartDate_ShouldThrowArgumentException()
        {
            // Arrange
            DateTime startDate = new DateTime(2019, 5, 1);
            DateTime terminationDate = new DateTime(2022, 5, 1);
            int sickDays = 2;
            string absenceReason = "betreuungsurlaub";
            DateTime? reasonStartDate = null;
            int yearsOfService = 3;

            // Act & Assert
            var ex = Assert.ThrowsException<ArgumentException>(() =>
                new Termination(startDate, terminationDate, sickDays, absenceReason, reasonStartDate, yearsOfService)
            );

            StringAssert.Contains(ex.Message, "A reason start date is required for the absence reason 'betreuungsurlaub'.");
        }

        [TestMethod]
        public void Termination_NegativeYearsOfService_ShouldThrowArgumentOutOfRangeException()
        {
            // Arrange
            DateTime startDate = new DateTime(2020, 1, 1);
            DateTime terminationDate = new DateTime(2023, 1, 1);
            int sickDays = 5;
            string absenceReason = "krankheit";
            DateTime? reasonStartDate = null;
            int yearsOfService = -2;

            // Act & Assert
            var ex = Assert.ThrowsException<ArgumentOutOfRangeException>(() =>
                new Termination(startDate, terminationDate, sickDays, absenceReason, reasonStartDate, yearsOfService)
            );

            StringAssert.Contains(ex.Message, "Years of service cannot be negative.");
        }

        [TestMethod]
        public void Termination_MilitaryServiceWithReasonStartDate_ShouldCreateInstance()
        {
            // Arrange
            DateTime startDate = new DateTime(2020, 1, 1);
            DateTime terminationDate = new DateTime(2023, 1, 1);
            int sickDays = 5;
            string absenceReason = "militärdienst";
            DateTime? reasonStartDate = new DateTime(2022, 12, 1);
            int yearsOfService = 3;

            // Act
            var termination = new Termination(startDate, terminationDate, sickDays, absenceReason, reasonStartDate, yearsOfService);

            // Assert
            Assert.AreEqual(absenceReason.ToLower(), termination.AbsenceReason, "AbsenceReason wurde nicht korrekt gesetzt.");
            Assert.AreEqual(reasonStartDate, termination.ReasonStartDate, "ReasonStartDate wurde nicht korrekt gesetzt.");
        }

        [TestMethod]
        public void Termination_BetreuungsurlaubWithReasonStartDate_ShouldCreateInstance()
        {
            // Arrange
            DateTime startDate = new DateTime(2019, 5, 1);
            DateTime terminationDate = new DateTime(2022, 5, 1);
            int sickDays = 2;
            string absenceReason = "betreuungsurlaub";
            DateTime? reasonStartDate = new DateTime(2021, 6, 15);
            int yearsOfService = 3;

            // Act
            var termination = new Termination(startDate, terminationDate, sickDays, absenceReason, reasonStartDate, yearsOfService);

            // Assert
            Assert.AreEqual(absenceReason.ToLower(), termination.AbsenceReason, "AbsenceReason wurde nicht korrekt gesetzt.");
            Assert.AreEqual(reasonStartDate, termination.ReasonStartDate, "ReasonStartDate wurde nicht korrekt gesetzt.");
        }

        #endregion

        #region SetAbsenceDetails Tests

        [TestMethod]
        public void SetAbsenceDetails_ValidParameters_ShouldUpdateProperties()
        {
            // Arrange
            var termination = new Termination(
                new DateTime(2020, 1, 1),
                new DateTime(2023, 1, 1),
                5,
                "krankheit",
                null,
                3);

            string newReason = "unfall";
            DateTime newStartDate = new DateTime(2022, 5, 1);
            DateTime? newEndDate = new DateTime(2022, 5, 15);

            // Act
            termination.SetAbsenceDetails(newReason, newStartDate, newEndDate);

            // Assert
            Assert.AreEqual(newReason.ToLower(), termination.AbsenceReason, "AbsenceReason wurde nach SetAbsenceDetails nicht korrekt aktualisiert.");
            Assert.AreEqual(newStartDate, termination.ReasonStartDate, "ReasonStartDate wurde nach SetAbsenceDetails nicht korrekt aktualisiert.");
            Assert.AreEqual(newEndDate, termination.ReasonEndDate, "ReasonEndDate wurde nach SetAbsenceDetails nicht korrekt aktualisiert.");
        }

        [TestMethod]
        public void SetAbsenceDetails_InvalidReason_ShouldThrowArgumentException()
        {
            // Arrange
            var termination = new Termination(
                new DateTime(2020, 1, 1),
                new DateTime(2023, 1, 1),
                5,
                "krankheit",
                null,
                3);

            string invalidReason = "invalid_reason";
            DateTime newStartDate = new DateTime(2022, 5, 1);

            // Act & Assert
            var ex = Assert.ThrowsException<ArgumentException>(() =>
                termination.SetAbsenceDetails(invalidReason, newStartDate)
            );

            StringAssert.Contains(ex.Message, $"The absence reason '{invalidReason}' is not recognized. Please choose a valid reason from the allowed list.");
        }

        [TestMethod]
        public void SetAbsenceDetails_EmptyReason_ShouldThrowArgumentException()
        {
            // Arrange
            var termination = new Termination(
                new DateTime(2020, 1, 1),
                new DateTime(2023, 1, 1),
                5,
                "krankheit",
                null,
                3);

            string emptyReason = "  ";
            DateTime newStartDate = new DateTime(2022, 5, 1);

            // Act & Assert
            var ex = Assert.ThrowsException<ArgumentException>(() =>
                termination.SetAbsenceDetails(emptyReason, newStartDate)
            );

            StringAssert.Contains(ex.Message, "The absence reason cannot be empty.");
        }

        #endregion

        #region CalculateTerminationDeadline Tests

        [TestMethod]
        public void CalculateTerminationDeadline_StandardCase_ShouldReturnCorrectDate()
        {
            // Arrange
            DateTime startDate = new DateTime(2020, 1, 1);
            DateTime terminationDate = new DateTime(2023, 1, 1);
            int sickDays = 10;
            string absenceReason = "krankheit";
            DateTime? reasonStartDate = null;
            int yearsOfService = 3;

            var termination = new Termination(startDate, terminationDate, sickDays, absenceReason, reasonStartDate, yearsOfService);

            // Act
            DateTime terminationDeadline = termination.CalculateTerminationDeadline();

            // Assert
            // Calculation:
            // yearsOfService = 3 => noticePeriodInMonths = 2 (1 < years < 10)
            // adjustedTerminationDate = terminationDate + sickDays = 2023-01-11
            // MustBeExtended check based on "krankheit" and yearsOfService =3
            // For "krankheit" and yearsOfService=3, extension if terminationNoticeDate <= 90 days
            // Since terminationNoticeDate is 2023-01-11 <= 2023-01-01 + 90 days = 2023-04-01, extension adds 90 days: 2023-01-11 + 90 days = 2023-04-11
            // endNoticePeriod = 2023-04-11 + 2 months = 2023-06-11
            // Return the last day of the month: 2023-06-30

            DateTime expected = new DateTime(2023, 6, 30);
            Assert.AreEqual(expected, terminationDeadline, "Die berechnete Kündigungsfrist ist falsch.");
        }

        [TestMethod]
        public void CalculateTerminationDeadline_WithSickDays_ShouldAdjustTerminationDate()
        {
            // Arrange
            DateTime startDate = new DateTime(2021, 6, 15);
            DateTime terminationDate = new DateTime(2023, 6, 14);
            int sickDays = 5;
            string absenceReason = "krankheit";
            DateTime? reasonStartDate = null;
            int yearsOfService = 2;

            var termination = new Termination(startDate, terminationDate, sickDays, absenceReason, reasonStartDate, yearsOfService);

            // Act
            DateTime terminationDeadline = termination.CalculateTerminationDeadline();

            // Assert
            // Calculation:
            // yearsOfService = 2 => noticePeriodInMonths = 2 (1 < years <10)
            // adjustedTerminationDate = terminationDate + sickDays = 2023-06-19
            // MustBeExtended: "krankheit" and yearsOfService=2, terminationNoticeDate <= 90 days from terminationDate
            // 2023-06-19 <= 2023-06-14 +90 days = 2023-09-12 => true, add 90 days: 2023-06-19 + 90 days = 2023-09-19
            // endNoticePeriod = 2023-09-19 + 2 months = 2023-11-19
            // Return the last day of the month: 2023-11-30

            DateTime expected = new DateTime(2023, 11, 30);
            Assert.AreEqual(expected, terminationDeadline, "Die berechnete Kündigungsfrist mit Krankheitstagen ist falsch.");
        }

        [TestMethod]
        public void CalculateTerminationDeadline_InvalidTerminationDate_ShouldThrowArgumentException()
        {
            // Arrange
            DateTime startDate = new DateTime(2020, 1, 1);
            DateTime terminationDate = new DateTime(2019, 12, 31); // terminationDate < startDate
            int sickDays = 5;
            string absenceReason = "krankheit";
            DateTime? reasonStartDate = null;
            int yearsOfService = 3;

            // Act & Assert
            var ex = Assert.ThrowsException<ArgumentException>(() =>
                new Termination(startDate, terminationDate, sickDays, absenceReason, reasonStartDate, yearsOfService)
            );

            StringAssert.Contains(ex.Message, "The start date cannot be after the termination date.");
        }

        [TestMethod]
        public void CalculateTerminationDeadline_NegativeSickDays_ShouldThrowArgumentOutOfRangeException()
        {
            // Arrange
            DateTime startDate = new DateTime(2020, 1, 1);
            DateTime terminationDate = new DateTime(2023, 1, 1);
            int sickDays = -5;
            string absenceReason = "krankheit";
            DateTime? reasonStartDate = null;
            int yearsOfService = 3;

            // Act & Assert
            var ex = Assert.ThrowsException<ArgumentOutOfRangeException>(() =>
                new Termination(startDate, terminationDate, sickDays, absenceReason, reasonStartDate, yearsOfService)
            );

            StringAssert.Contains(ex.Message, "Sick days cannot be negative.");
        }

        #endregion

        #region MustBeExtended Tests

        [TestMethod]
        public void MustBeExtended_MilitaryServiceWithinBuffer_ShouldReturnTrue()
        {
            // Arrange
            DateTime startDate = new DateTime(2020, 1, 1);
            DateTime terminationDate = new DateTime(2023, 1, 1);
            int sickDays = 0;
            string absenceReason = "militärdienst";
            DateTime? reasonStartDate = new DateTime(2022, 12, 12); // Ends on 2022-12-23 (12 + 11 days)
            int yearsOfService = 3;

            var termination = new Termination(startDate, terminationDate, sickDays, absenceReason, reasonStartDate, yearsOfService);

            // terminationNoticeDate = terminationDate = 2023-01-01
            // militaryServiceEnd = 2022-12-23
            // bufferPeriodStart = 2022-12-23 - 28 days = 2022-11-25
            // bufferPeriodEnd = 2022-12-23 + 28 days = 2023-01-20
            // 2023-01-01 is within the buffer, hence should be extended

            // Act
            bool mustBeExtended = termination.MustBeExtended(terminationDate);

            // Assert
            Assert.IsTrue(mustBeExtended, "Die Kündigung sollte verlängert werden.");
        }

        [TestMethod]
        public void MustBeExtended_SickLeaveYears1Within30Days_ShouldReturnTrue()
        {
            // Arrange
            DateTime startDate = new DateTime(2022, 1, 1);
            DateTime terminationDate = new DateTime(2023, 1, 1);
            int sickDays = 0;
            string absenceReason = "krankheit";
            DateTime? reasonStartDate = null;
            int yearsOfService = 1;

            var termination = new Termination(startDate, terminationDate, sickDays, absenceReason, reasonStartDate, yearsOfService);

            // terminationNoticeDate = terminationDate = 2023-01-01
            // terminationNoticeDate <= terminationDate.AddDays(30) = 2023-01-31
            // Hence, should be extended

            // Act
            bool mustBeExtended = termination.MustBeExtended(terminationDate);

            // Assert
            Assert.IsTrue(mustBeExtended, "Die Kündigung sollte verlängert werden.");
        }

        [TestMethod]
        public void MustBeExtended_PregnancyWithinPostpartum_ShouldReturnTrue()
        {
            // Arrange
            DateTime startDate = new DateTime(2021, 1, 1);
            DateTime terminationDate = new DateTime(2023, 1, 1);
            int sickDays = 0;
            string absenceReason = "schwangerschaft";
            DateTime? reasonStartDate = new DateTime(2022, 12, 1); // Birth date
            int yearsOfService = 2;

            var termination = new Termination(startDate, terminationDate, sickDays, absenceReason, reasonStartDate, yearsOfService);

            // terminationNoticeDate = terminationDate = 2023-01-01
            // postpartumEnd = 2022-12-01 + 112 days = 2023-04-23
            // 2023-01-01 <= 2023-04-23, hence should be extended

            // Act
            bool mustBeExtended = termination.MustBeExtended(terminationDate);

            // Assert
            Assert.IsTrue(mustBeExtended, "Die Kündigung sollte verlängert werden.");
        }

        [TestMethod]
        public void MustBeExtended_AccidentWithinYearsOfService_ShouldReturnTrue()
        {
            // Arrange
            DateTime startDate = new DateTime(2020, 1, 1);
            DateTime terminationDate = new DateTime(2023, 6, 1);
            int sickDays = 0;
            string absenceReason = "unfall";
            DateTime? reasonStartDate = new DateTime(2023, 3, 1);
            int yearsOfService = 4;

            var termination = new Termination(startDate, terminationDate, sickDays, absenceReason, reasonStartDate, yearsOfService);

            // terminationNoticeDate = terminationDate = 2023-06-01
            // For "unfall" and yearsOfService=4, terminationNoticeDate <= terminationDate.AddDays(90) = 2023-08-30
            // 2023-06-01 <= 2023-08-30, hence should be extended

            // Act
            bool mustBeExtended = termination.MustBeExtended(terminationDate);

            // Assert
            Assert.IsTrue(mustBeExtended, "Die Kündigung sollte verlängert werden.");
        }

        [TestMethod]
        public void MustBeExtended_AssistanceActionWithinBuffer_ShouldReturnTrue()
        {
            // Arrange
            DateTime startDate = new DateTime(2021, 1, 1);
            DateTime terminationDate = new DateTime(2023, 1, 1);
            string absenceReason = "hilfsaktion";
            DateTime reasonEndDate = new DateTime(2022, 12, 15);
            int yearsOfService = 5;

            var termination = new Termination(
                startDate,
                terminationDate,
                0,
                absenceReason,
                null,
                yearsOfService);
            termination.SetAbsenceDetails(absenceReason, new DateTime(2022, 12, 1), reasonEndDate);

            // terminationNoticeDate = terminationDate = 2023-01-01
            // bufferPeriodEnd = 2022-12-15 + 28 days = 2023-01-12
            // 2023-01-01 <= 2023-01-12, hence should be extended

            // Act
            bool mustBeExtended = termination.MustBeExtended(terminationDate);

            // Assert
            Assert.IsTrue(mustBeExtended, "Die Kündigung sollte verlängert werden.");
        }

        [TestMethod]
        public void MustBeExtended_NoExtensionRequired_ShouldReturnFalse()
        {
            // Arrange
            DateTime startDate = new DateTime(2018, 1, 1);
            DateTime terminationDate = new DateTime(2023, 6, 1);
            int sickDays = 0;
            string absenceReason = "krankheit";
            DateTime? reasonStartDate = null;
            int yearsOfService = 6;

            var termination = new Termination(
                startDate,
                terminationDate,
                sickDays,
                absenceReason,
                reasonStartDate,
                yearsOfService);

            // terminationNoticeDate = terminationDate.AddDays(181) = 2023-06-01 + 181 days = 2023-11-29
            // For "krankheit" and yearsOfService=6, terminationNoticeDate <= terminationDate.AddDays(180) = 2023-11-28
            // 2023-11-29 > 2023-11-28, hence should not be extended

            // Act
            bool mustBeExtended = termination.MustBeExtended(terminationDate.AddDays(181));

            // Assert
            Assert.IsFalse(mustBeExtended, "Die Kündigung sollte nicht verlängert werden.");
        }

        #endregion

        #region CalculateExtension Tests

        [TestMethod]
        public void CalculateExtension_SickLeaveYears1_ShouldAdd30Days()
        {
            // Arrange
            DateTime terminationNoticeDate = new DateTime(2023, 1, 1);
            int yearsOfService = 1;

            var termination = new Termination(
                new DateTime(2022, 1, 1),
                terminationNoticeDate,
                0,
                "krankheit",
                null,
                yearsOfService);

            // Act
            DateTime extendedDate = termination.CalculateExtension(terminationNoticeDate);

            // Assert
            DateTime expected = terminationNoticeDate.AddDays(30);
            Assert.AreEqual(expected, extendedDate, "Die verlängerte Kündigungsfrist ist falsch.");
        }

        [TestMethod]
        public void CalculateExtension_Pregnancy_ShouldAdd112Days()
        {
            // Arrange
            DateTime terminationNoticeDate = new DateTime(2023, 1, 1);
            DateTime reasonStartDate = new DateTime(2022, 12, 1);
            string absenceReason = "schwangerschaft";
            int yearsOfService = 2;

            var termination = new Termination(
                new DateTime(2021, 1, 1),
                terminationNoticeDate,
                0,
                absenceReason,
                reasonStartDate,
                yearsOfService);

            // Act
            DateTime extendedDate = termination.CalculateExtension(terminationNoticeDate);

            // Assert
            DateTime expected = terminationNoticeDate.AddDays(112);
            Assert.AreEqual(expected, extendedDate, "Die verlängerte Kündigungsfrist für Schwangerschaft ist falsch.");
        }

        [TestMethod]
        public void CalculateExtension_MilitaryService_ShouldAdd28DaysAfterServiceEnd()
        {
            // Arrange
            DateTime terminationNoticeDate = new DateTime(2023, 1, 25); // Geändertes Datum, um extendedDate zuzulassen
            DateTime reasonStartDate = new DateTime(2022, 12, 12);
            string absenceReason = "militärdienst";
            int yearsOfService = 3;

            var termination = new Termination(
                startDate: new DateTime(2021, 1, 1),
                terminationDate: terminationNoticeDate,
                sickDays: 0,
                absenceReason: absenceReason,
                reasonStartDate: reasonStartDate,
                yearsOfService: yearsOfService
            );

            // Act
            DateTime extendedDate = termination.CalculateExtension(terminationNoticeDate);

            // Assert
            // Calculation:
            // militaryServiceEnd = reasonStartDate + 11 days = 2022-12-23
            // extendedDate = militaryServiceEnd + 28 days = 2023-01-20

            DateTime expected = reasonStartDate.AddDays(11 + 28); // 2022-12-12 + 11 + 28 = 2023-01-20
            Assert.AreEqual(expected, extendedDate, "Die verlängerte Kündigungsfrist für Militärdienst ist falsch.");
        }



        [TestMethod]
        public void CalculateExtension_Betreuungsurlaub_ShouldAdd6Months()
        {
            // Arrange
            DateTime terminationNoticeDate = new DateTime(2023, 1, 1);
            DateTime reasonStartDate = new DateTime(2022, 7, 1);
            string absenceReason = "betreuungsurlaub";
            int yearsOfService = 5;

            var termination = new Termination(
                new DateTime(2019, 5, 1),
                terminationNoticeDate,
                0,
                absenceReason,
                reasonStartDate,
                yearsOfService);

            // Act
            DateTime extendedDate = termination.CalculateExtension(terminationNoticeDate);

            // Assert
            DateTime expected = reasonStartDate.AddMonths(6); // 2022-07-01 +6 months = 2023-01-01
            Assert.AreEqual(expected, extendedDate, "Die verlängerte Kündigungsfrist für Betreuungsurlaub ist falsch.");
        }

        [TestMethod]
        public void CalculateExtension_Hilfsaktion_ShouldAdd28Days()
        {
            // Arrange
            DateTime terminationNoticeDate = new DateTime(2023, 1, 1);
            DateTime reasonEndDate = new DateTime(2022, 12, 15);
            string absenceReason = "hilfsaktion";
            int yearsOfService = 5;

            var termination = new Termination(
                new DateTime(2021, 1, 1),
                terminationNoticeDate,
                0,
                absenceReason,
                null,
                yearsOfService);
            termination.SetAbsenceDetails(absenceReason, new DateTime(2022, 12, 1), reasonEndDate);

            // Act
            DateTime extendedDate = termination.CalculateExtension(terminationNoticeDate);

            // Assert
            DateTime expected = terminationNoticeDate.AddDays(28);
            Assert.AreEqual(expected, extendedDate, "Die verlängerte Kündigungsfrist für Hilfsaktion ist falsch.");
        }

        [TestMethod]
        public void Termination_InvalidReasonStartDate_ShouldThrowArgumentException()
        {
            // Arrange
            DateTime terminationNoticeDate = new DateTime(2023, 1, 1);
            string absenceReason = "militärdienst";
            int yearsOfService = 2;

            // Act & Assert
            var ex = Assert.ThrowsException<ArgumentException>(() =>
                new Termination(
                    startDate: new DateTime(2020, 1, 1),
                    terminationDate: terminationNoticeDate,
                    sickDays: 0,
                    absenceReason: absenceReason,
                    reasonStartDate: null, // Fehlen des ReasonStartDate
                    yearsOfService: yearsOfService
                )
            );

            StringAssert.Contains(ex.Message, $"A reason start date is required for the absence reason '{absenceReason}'.");
        }


        #endregion

        #region IsTerminationInvalid Tests

        [TestMethod]
        public void IsTerminationInvalid_DuringMilitaryService_ShouldReturnTrue()
        {
            // Arrange
            DateTime startDate = new DateTime(2020, 1, 1);
            DateTime terminationDate = new DateTime(2022, 12, 23);
            string absenceReason = "militärdienst";
            DateTime reasonStartDate = new DateTime(2022, 12, 12);
            int yearsOfService = 2;

            var termination = new Termination(
                startDate,
                terminationDate,
                0,
                absenceReason,
                reasonStartDate,
                yearsOfService);

            // Act
            bool isInvalid = termination.IsTerminationInvalid();

            // Assert
            Assert.IsTrue(isInvalid, "Die Kündigung sollte als nichtig betrachtet werden.");
        }

        [TestMethod]
        public void IsTerminationInvalid_DuringPregnancyProtection_ShouldReturnTrue()
        {
            // Arrange
            DateTime startDate = new DateTime(2021, 1, 1);
            DateTime terminationDate = new DateTime(2023, 3, 1);
            string absenceReason = "schwangerschaft";
            DateTime reasonStartDate = new DateTime(2023, 1, 1); // Birth date
            int yearsOfService = 3;

            var termination = new Termination(
                startDate,
                terminationDate,
                0,
                absenceReason,
                reasonStartDate,
                yearsOfService);

            // Act
            bool isInvalid = termination.IsTerminationInvalid();

            // Assert
            Assert.IsTrue(isInvalid, "Die Kündigung sollte als nichtig betrachtet werden.");
        }

        [TestMethod]
        public void IsTerminationInvalid_NotDuringProtection_ShouldReturnFalse()
        {
            // Arrange
            DateTime startDate = new DateTime(2018, 1, 1);
            DateTime terminationDate = new DateTime(2023, 6, 1);
            string absenceReason = "krankheit";
            DateTime? reasonStartDate = null;
            int yearsOfService = 6;

            var termination = new Termination(
                startDate,
                terminationDate,
                0,
                absenceReason,
                reasonStartDate,
                yearsOfService);

            // Act
            bool isInvalid = termination.IsTerminationInvalid();

            // Assert
            Assert.IsFalse(isInvalid, "Die Kündigung sollte nicht als nichtig betrachtet werden.");
        }

        [TestMethod]
        public void IsTerminationInvalid_DuringCareLeave_ShouldReturnTrue()
        {
            // Arrange
            DateTime startDate = new DateTime(2019, 1, 1);
            DateTime terminationDate = new DateTime(2023, 6, 1);
            string absenceReason = "betreuungsurlaub";
            DateTime reasonStartDate = new DateTime(2023, 1, 1);
            int yearsOfService = 5;

            var termination = new Termination(
                startDate,
                terminationDate,
                0,
                absenceReason,
                reasonStartDate,
                yearsOfService);

            // Act
            bool isInvalid = termination.IsTerminationInvalid();

            // Assert
            Assert.IsTrue(isInvalid, "Die Kündigung sollte als nichtig betrachtet werden.");
        }

        [TestMethod]
        public void IsTerminationInvalid_DuringAidActionProtection_ShouldReturnTrue()
        {
            // Arrange
            DateTime startDate = new DateTime(2021, 1, 1);
            DateTime terminationDate = new DateTime(2023, 1, 10); // Within buffer period
            string absenceReason = "hilfsaktion";
            DateTime reasonEndDate = new DateTime(2022, 12, 15);
            int yearsOfService = 4;

            var termination = new Termination(
                startDate,
                terminationDate,
                0,
                absenceReason,
                null,
                yearsOfService);
            termination.SetAbsenceDetails(absenceReason, new DateTime(2022, 12, 1), reasonEndDate);

            // Act
            bool isInvalid = termination.IsTerminationInvalid();

            // Assert
            Assert.IsTrue(isInvalid, "Die Kündigung sollte als nichtig betrachtet werden.");
        }

        #endregion
    }
}
