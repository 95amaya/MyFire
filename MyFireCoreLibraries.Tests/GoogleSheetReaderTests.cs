using AutoMapper;
using Bogus;
using FluentAssertions;
using Moq;
using MyFireCoreLibraries;

namespace MyFireCoreLibrariesTests;

public class GoogleSheetReaderTests
{
    private readonly Mock<IMapper> _mockMapper;
    private readonly Mock<ISheetClient> _mockSheetClient;

    public GoogleSheetReaderTests()
    {
        _mockMapper = new Mock<IMapper>(MockBehavior.Strict);
        _mockSheetClient = new Mock<ISheetClient>(MockBehavior.Loose);
    }

    [Fact]
    public void ShouldReturnData()
    {
        // setup
        var spreadsheetId = (new Guid()).ToString();
        var range = "A:E";

        var fakerTestData = new Faker<TestClass>();
        var expectedDataList = new List<TestClass>()
        {
            fakerTestData.Generate(),
            fakerTestData.Generate()
        };

        var rawData = new List<IList<Object>>()
        {
            new List<Object>()
        };
        

        _mockMapper.Setup(mapper => mapper.Map<IList<TestClass>>(It.IsAny<IList<IList<Object>>>()))
            .Returns(expectedDataList);
        _mockSheetClient.Setup(sheetClient => sheetClient.GetValues(It.IsAny<string>(), It.IsAny<string>()))
            .Returns(rawData);
        
        var googleSheetReader = new GoogleSheetReader(_mockMapper.Object, _mockSheetClient.Object);

        // act
        var data = googleSheetReader.ReadFrom<TestClass>(spreadsheetId, range);

        // assert
        data.Should().Equal(expectedDataList);
    }
    
    [Fact]
    public void ShouldNotReturnData()
    {
        // setup
        var spreadsheetId = (new Guid()).ToString();
        var range = "A:E";

        var fakerTestData = new Faker<TestClass>();
        var expectedDataList = new List<TestClass>()
        {
            fakerTestData.Generate(),
            fakerTestData.Generate()
        };

        var rawData = new List<IList<Object>>();
        
        _mockMapper.Setup(mapper => mapper.Map<IList<TestClass>>(It.IsAny<IList<IList<Object>>>()))
            .Returns(expectedDataList);
        _mockSheetClient.Setup(sheetClient => sheetClient.GetValues(It.IsAny<string>(), It.IsAny<string>()))
            .Returns(rawData);
        
        var googleSheetReader = new GoogleSheetReader(_mockMapper.Object, _mockSheetClient.Object);

        // act
        var data = googleSheetReader.ReadFrom<TestClass>(spreadsheetId, range);

        // assert
        data.Should().BeEmpty();
    }

    public class TestClass {}
}