using System.Linq;
using System.Threading.Tasks;

using FluentAssertions;

using TasksBoard.Backend.Features.Boards;

using Xunit;
using Xunit.Abstractions;

namespace TasksBoard.Tests.Features.Boards
{
    public class ListTest : BackendTestBase
    {
        public ListTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public async Task List_FilterByName_ReturnListOfBoard()
        {
            var userId = await CreateUser();

            var board1Name= "name1";
            var board1Id = await BoardTestHelper.CreateBoard(ContextInjector.WriteContext, userId, board1Name);

            var board2Name = "name1122";
            var board2Id = await BoardTestHelper.CreateBoard(ContextInjector.WriteContext, userId, board2Name);

            var board3Name = "name2233";
            var board3Id = await BoardTestHelper.CreateBoard(ContextInjector.WriteContext, userId, board3Name);

            var query = new List.Query("2");
            var boardsEnvelope = await SendAsync(query);

            boardsEnvelope.BoardsCount.Should().Be(2);

            var board2 = boardsEnvelope.Boards.FirstOrDefault(t => t.Id == board2Id);
            board2.Should().NotBeNull();
            board2.Name.Should().BeEquivalentTo(board2Name);

            var board3 = boardsEnvelope.Boards.FirstOrDefault(t => t.Id == board3Id);
            board3.Should().NotBeNull();
            board3.Name.Should().BeEquivalentTo(board3Name);
        }
    }
}