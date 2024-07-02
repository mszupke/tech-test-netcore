using System;

namespace Todo;

public class UserWithGravatar
{
    public string Email { get; set; }
    public string Name { get; set; }
    public string AvatarUrl { get; set; }

    public bool IsAuthenticated { get; set; } = true;

    public static UserWithGravatar Empty { get; } = new UserWithGravatar { IsAuthenticated = false };
}
