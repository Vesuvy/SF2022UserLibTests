namespace SF2022UserLib.Tests
{
    [TestClass]
    public class CalculationsTests
    {
        [TestMethod]
        public void AvailablePeriods_NoConsultations_AllDayFree()
        {
            // Arrange
            var calculations = new Calculations();
            TimeSpan[] startTimes = [];
            int[] durations = [];
            TimeSpan beginWorkingTime = new (9, 0, 0);
            TimeSpan endWorkingTime = new (17, 0, 0);
            int consultationTime = 60;

            // Act
            var result = calculations.AvailablePeriods(startTimes, durations, beginWorkingTime, endWorkingTime, consultationTime);

            // Assert
            Assert.AreEqual(8, result.Length);
            Assert.AreEqual("09:00-10:00", result[0]);
            Assert.AreEqual("16:00-17:00", result[7]);
        }

        [TestMethod]
        public void AvailablePeriods_WithSingleConsultation_GapsAroundConsultation()
        {
            // Arrange
            var calculations = new Calculations();
            TimeSpan[] startTimes = [new (10, 0, 0)];
            int[] durations = [60];
            TimeSpan beginWorkingTime = new (9, 0, 0);
            TimeSpan endWorkingTime = new (17, 0, 0);
            int consultationTime = 60;

            // Act
            var result = calculations.AvailablePeriods(startTimes, durations, beginWorkingTime, endWorkingTime, consultationTime);

            // Assert
            Assert.AreEqual(7, result.Length);
            Assert.AreEqual("09:00-10:00", result[0]);
            Assert.AreEqual("11:00-12:00", result[1]);
            Assert.AreEqual("16:00-17:00", result[6]);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AvailablePeriods_NullStartTimes_ThrowsArgumentNullException()
        {
            // Arrange
            var calculations = new Calculations();
            TimeSpan[]? startTimes = null;
            int[] durations = [60];
            TimeSpan beginWorkingTime = new(9, 0, 0);
            TimeSpan endWorkingTime = new(17, 0, 0);
            int consultationTime = 60;

            // Act
            calculations.AvailablePeriods(startTimes, durations, beginWorkingTime, endWorkingTime, consultationTime);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AvailablePeriods_NullDurations_ThrowsArgumentNullException()
        {
            // Arrange
            var calculations = new Calculations();
            TimeSpan[] startTimes = [new TimeSpan(10, 0, 0)];
            int[]? durations = null;
            TimeSpan beginWorkingTime = new(9, 0, 0);
            TimeSpan endWorkingTime = new(17, 0, 0);
            int consultationTime = 60;

            // Act
            calculations.AvailablePeriods(startTimes, durations, beginWorkingTime, endWorkingTime, consultationTime);
        }

        [TestMethod]
        [ExpectedException(typeof(ArrayMismatchException))]
        public void AvailablePeriods_MismatchedArrays_ThrowsArrayMismatchException()
        {
            // Arrange
            var calculations = new Calculations();
            TimeSpan[] startTimes = [new TimeSpan(10, 0, 0)];
            int[] durations = [60, 30];
            TimeSpan beginWorkingTime = new(9, 0, 0);
            TimeSpan endWorkingTime = new(17, 0, 0);
            int consultationTime = 60;

            // Act
            calculations.AvailablePeriods(startTimes, durations, beginWorkingTime, endWorkingTime, consultationTime);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void AvailablePeriods_NonPositiveConsultationTime_ThrowsArgumentException()
        {
            // Arrange
            var calculations = new Calculations();
            TimeSpan[] startTimes = [new TimeSpan(10, 0, 0)];
            int[] durations = [60];
            TimeSpan beginWorkingTime = new(9, 0, 0);
            TimeSpan endWorkingTime = new(17, 0, 0);
            int consultationTime = 0;

            // Act
            calculations.AvailablePeriods(startTimes, durations, beginWorkingTime, endWorkingTime, consultationTime);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void AvailablePeriods_BeginWorkingTimeAfterEndWorkingTime_ThrowsArgumentOutOfRangeException()
        {
            // Arrange
            var calculations = new Calculations();
            TimeSpan[] startTimes = [new TimeSpan(10, 0, 0)];
            int[] durations = [60];
            TimeSpan beginWorkingTime = new(17, 0, 0);
            TimeSpan endWorkingTime = new(9, 0, 0);
            int consultationTime = 60;

            // Act
            calculations.AvailablePeriods(startTimes, durations, beginWorkingTime, endWorkingTime, consultationTime);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void AvailablePeriods_StartTimeOutsideWorkingHours_ThrowsArgumentOutOfRangeException()
        {
            // Arrange
            var calculations = new Calculations();
            TimeSpan[] startTimes = [new TimeSpan(8, 0, 0)];
            int[] durations = [60];
            TimeSpan beginWorkingTime = new(9, 0, 0);
            TimeSpan endWorkingTime = new(17, 0, 0);
            int consultationTime = 60;

            // Act
            calculations.AvailablePeriods(startTimes, durations, beginWorkingTime, endWorkingTime, consultationTime);
        }

        [TestMethod]
        public void AvailablePeriods_ConsultationLongerThanWorkingDay_NoAvailablePeriods()
        {
            // Arrange
            var calculations = new Calculations();
            TimeSpan[] startTimes = [];
            int[] durations = [];
            TimeSpan beginWorkingTime = new(9, 0, 0);
            TimeSpan endWorkingTime = new(9, 30, 0);
            int consultationTime = 60;

            // Act
            var result = calculations.AvailablePeriods(startTimes, durations, beginWorkingTime, endWorkingTime, consultationTime);

            // Assert
            Assert.AreEqual(0, result.Length);
        }

        [TestMethod]
        public void AvailablePeriods_MultipleConsultations_ValidAvailablePeriods()
        {
            // Arrange
            var calculations = new Calculations();
            TimeSpan[] startTimes = [new TimeSpan(10, 0, 0), new TimeSpan(14, 0, 0)]; // начало консультаций 1-ая, 2-ая
            int[] durations = [60, 90]; // длительности двух консульт.
            TimeSpan beginWorkingTime = new(9, 0, 0); // начало раб. дня
            TimeSpan endWorkingTime = new(17, 0, 0); // конец раб. дня
            int consultationTime = 60; // длительность одной конс.

            // Act
            var result = calculations.AvailablePeriods(startTimes, durations, beginWorkingTime, endWorkingTime, consultationTime);

            // Assert
            Assert.AreEqual(6, result.Length);
            Assert.AreEqual("09:00-10:00", result[0]);
            Assert.AreEqual("11:00-12:00", result[1]);
            Assert.AreEqual("12:00-13:00", result[2]);
            Assert.AreEqual("13:00-14:00", result[3]);
            Assert.AreEqual("15:30-16:30", result[4]);
            Assert.AreEqual("16:30-17:00", result[5]);
        }
    }
}