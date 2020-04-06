using System;

using FluentAssertions;

using TasksBoard.Backend.Features.Boards;
using TasksBoard.Backend.Infrastructure.Errors;

using Xunit;
using Xunit.Abstractions;

using Task = System.Threading.Tasks.Task;

namespace TasksBoard.Tests.Features.Boards
{
    public class DetailsTest : BackendTestBase
    {
        public DetailsTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public async Task Details_CheckExistingBoard_ReturnBoard()
        {
            var userId = await CreateUser();
            var boardName = "name";
            var boardId = await BoardTestHelper.CreateBoard(ContextInjector.WriteContext, userId, boardName);

            var query = new Details.Query(boardId);
            var boardEnvelope = await SendAsync(query);

            boardEnvelope.Should().NotBeNull();
            boardEnvelope.Board.Should().NotBeNull();
            boardEnvelope.Board.Name.Should().BeEquivalentTo(boardName);
            boardEnvelope.Board.OwnerId.Should().Be(userId);
        }

        [Fact]
        public async Task Details_CheckNotExistingBoard_ReturnException()
        {
            var userId = await CreateUser();
            var boardId = await BoardTestHelper.CreateBoard(ContextInjector.WriteContext, userId);

            var rndNotExistingGuid = Guid.NewGuid();
            while (rndNotExistingGuid == boardId)
                rndNotExistingGuid = Guid.NewGuid();

            var query = new Details.Query(rndNotExistingGuid);
            Func<Task> getBoardDetailFunc = async () => await SendAsync(query);
            getBoardDetailFunc.Should().Throw<RestException>();
        }
    }
}