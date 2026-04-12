using Application.Users.Responses;

namespace WebApi.Features.Users;

public record UserPictureResponse(
    int PictureId,
    string OriginalRelativePath,
    string SmallRelativePath,
    string MediumRelativePath,
    string LargeRelativePath,
    DateTime AddedAt)
{
    public static UserPictureResponse FromApplicationResponse(ApplicationUserPicture picture)
    {
        return new UserPictureResponse(
            picture.PictureId,
            picture.OriginalRelativePath,
            picture.SmallRelativePath,
            picture.MediumRelativePath,
            picture.LargeRelativePath,
            picture.AddedAt);
    }
}
