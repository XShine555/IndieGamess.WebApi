using Domain.Entities;

namespace WebApi.DataTransferObjects.AdminGame.Responses
{
    public record GameStorePictureAdminSummary(
        Guid PictureId,
        string OriginalPictureKey,
        string SmallPictureKey,
        string SmallPictureUrl,
        string MediumPictureKey,
        string MediumPictureUrl,
        string LargePictureKey,
        string LargePictureUrl,
        GamePictureProcessingStatus ProcessingStatus,
        DateTime AddedAt);
}
