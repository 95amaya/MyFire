using AutoMapper;
using Bogus;
using Bogus.DataSets;
using FluentAssertions;
using Moq;
using Services.CoreLibraries;
using System.Globalization;

namespace CoreLibrariesTests;

public class CsvReaderTests
{
    private readonly IMapper _mapper;

    public CsvReaderTests()
    {
        _mapper = new MapperConfiguration(cfg =>
        {
            _ = cfg.CreateMap<IEnumerable<string>, TestClass>()
                .ForMember(dest => dest.DateProp, act => act.MapFrom(src => DateTime.ParseExact(src.ElementAtOrDefault(0) as string, "yyyy-MM-dd", CultureInfo.InvariantCulture)))
                .ForMember(dest => dest.DecimalProp, act => act.MapFrom(src => src.ElementAt(1)))
                .ForMember(dest => dest.EnumProp, act => act.MapFrom(src => Enum.Parse<TestEnum>(src.ElementAtOrDefault(2) as string)))
                .ForMember(dest => dest.StringProp, act => act.MapFrom(src => src.ElementAtOrDefault(3)))
                .ForMember(dest => dest.BoolProp, act => act.MapFrom(src => src.ElementAtOrDefault(4)));
        }).CreateMapper();
    }

    [Theory]
    [InlineData(0, @"2023-08-03,-25.44,FOO,""foo-bar #1234"",True")]
    [InlineData(1, @"2023-08-03,25.44,BAR,""foo-bar #1234, comma""")]
    [InlineData(2, @"2023-08-03,,,""foo-bar #1234, comma""")] // FIX: Failing test
    public void ShouldReadCsvRow(int index, string row)
    {
        // setup
        var csvReader = new CsvReader(_mapper);
        using var reader = new StreamReader(GenerateStreamFromString(row));

        // act
        var data = csvReader.Read<TestClass>(reader).FirstOrDefault();

        // assert
        data.Should().BeEquivalentTo(ShouldReadCsvRowList.ElementAt(index));
    }

    // https://stackoverflow.com/questions/1879395/how-do-i-generate-a-stream-from-a-string
    private Stream GenerateStreamFromString(string s)
    {
        var stream = new MemoryStream();
        var writer = new StreamWriter(stream);
        writer.Write(s);
        writer.Flush();
        stream.Position = 0;
        return stream;
    }

    private readonly List<TestClass> ShouldReadCsvRowList = new()
    {
        new TestClass
        {
            DateProp = new DateTime(2023, 08, 03),
            DecimalProp = -25.44M,
            EnumProp = TestEnum.FOO,
            StringProp = "foo-bar #1234",
            BoolProp = true
        },
        new TestClass
        {
            DateProp = new DateTime(2023, 08, 03),
            DecimalProp = 25.44M,
            EnumProp = TestEnum.BAR,
            StringProp = "foo-bar #1234, comma",
            BoolProp = false
        },
        new TestClass
        {
            DateProp = new DateTime(2023, 08, 03),
            DecimalProp = 0,
            EnumProp = TestEnum.BAR,
            StringProp = "foo-bar #1234, comma",
            BoolProp = false
        },
    };

    public enum TestEnum
    {
        FOO = 0,
        BAR = 1,
    }
    public class TestClass
    {
        public DateTime DateProp { get; set; }
        public decimal DecimalProp { get; set; }
        public TestEnum EnumProp { get; set; }
        public string StringProp { get; set; } = string.Empty;
        public bool BoolProp { get; set; }
    }
}