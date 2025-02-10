using core.domain.types;

namespace unit_tests.core.domain.types
{
    public class DataValidaTests
    {
        [Fact]
        public void ConstrutorComSucesso()
        {
            DateTime validDate = new(2024, 1, 2, 0, 0, 0, DateTimeKind.Utc);
            DataValida dataValida = new(validDate);

            Assert.Equal(validDate, dataValida.Valor);
        }

        [Fact]
        public void ConstrutorLancaExceptionComDataMenorQueOConsiderado()
        {
            DateTime invalidDate = new(2023, 12, 31, 0, 0, 0, DateTimeKind.Utc);

            Assert.Throws<ArgumentException>(() => new DataValida(invalidDate));
        }

        [Fact]
        public void ConversaoDeDateTimeParaDataValida()
        {
            DateTime validDate = new(2024, 1, 2, 0, 0, 0, DateTimeKind.Utc);
            DataValida dataValida = validDate;

            Assert.Equal(validDate, dataValida.Valor);
        }

        [Fact]
        public void ConversaoDeDataValidaParaDateTime()
        {
            DateTime validDate = new(2024, 1, 2, 0, 0, 0, DateTimeKind.Utc);
            DataValida dataValida = new(validDate);
            DateTime dateTime = dataValida;

            Assert.Equal(validDate, dateTime);
        }

        [Fact]
        public void EqualsRetornaTrueComOsMesmosValores()
        {
            DateTime validDate = new(2024, 1, 2, 0, 0, 0, DateTimeKind.Utc);
            DataValida dataValida1 = new(validDate);
            DataValida dataValida2 = new(validDate);

            Assert.True(dataValida1.Equals(dataValida2));
        }

        [Fact]
        public void EqualsRetornaFalseComValoresDiferentes()
        {
            DataValida dataValida1 = new(new DateTime(2024, 1, 2, 0, 0, 0, DateTimeKind.Utc));
            DataValida dataValida2 = new(new DateTime(2024, 1, 3, 0, 0, 0, DateTimeKind.Utc));

            Assert.False(dataValida1.Equals(dataValida2));
        }
    }
}