namespace LinkedOut.Common.Domain;

public record SwaggerProperties
{
    /**
     * 文档标题
     */
    public string Title { get; init; } = null!;

    /**
     * 文档描述
     */
    public string Description { get; init; } = null!;

    /**
     * 文档版本
     */
    public string Version { get; init; } = null!;

    /**
     * 文档联系人姓名
     */
    public string ContactName { get; init; } = null!;

    /**
     * 文档联系人网址
     */
    public Uri ContactUrl { get; init; } = null!;

    /**
     * 文档联系人邮箱
     */
    public string ContactEmail { get; init; } = null!;
}