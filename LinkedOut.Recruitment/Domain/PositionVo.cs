﻿using LinkedOut.Common.Feign.User.Dto;
using LinkedOut.DB.Entity;

namespace LinkedOut.Recruitment.Domain;

public class PositionVo
{
    public UserDto UserDto { get; set; } = null!;

    public Position Position { get; set; } = null!;
}