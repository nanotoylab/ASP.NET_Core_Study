using Xunit;
using ASP.NET_Core_Study.Pages;

namespace ASP.NET_Core_Study.Tests.Application
{
    public class FeedbackUnitTests
    {
        // Arrange 用ヘルパー
        private static FeedbackModel CreateModel(string email = "", string message = "")
        {
            var model = new FeedbackModel
            {
                Email = email,
                Message = message
            };
            return model;
        }

        [Fact(DisplayName = "OnPost: 正しい入力で送信に成功する（単体）")]
        public void OnPost_ValidInput_Succeeds()
        {
            // Arrange
            var model = CreateModel("user@example.com", "これは十分に長いメッセージです。");

            // Act
            var result = model.OnPost();

            // Assert
            // PageResult 型チェックを外して、モデルの状態で検証することで余計な参照を避ける
            Assert.True(model.SubmissionSucceeded);
            Assert.True(model.ModelState.IsValid);
            Assert.NotNull(result); // 副作用の戻り値があることを簡潔に確認
        }

        [Fact(DisplayName = "OnPost: 入力不足で ModelState にエラーが入る（単体）")]
        public void OnPost_InvalidInput_Fails()
        {
            // Arrange: メール不正、メッセージ短い
            var model = CreateModel("invalid-email", "短い");

            // Act
            var result = model.OnPost();

            // Assert
            Assert.False(model.SubmissionSucceeded);
            Assert.False(model.ModelState.IsValid);
            Assert.True(model.ModelState.ErrorCount >= 1);
            Assert.NotNull(result);
        }
    }
}