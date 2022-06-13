namespace LinkedOut.DB.Entity
{
    public class AppFile
    {
        /// <summary>
        /// 文件Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 文件url
        /// </summary>
        public string Url { get; set; } = null!;

        /// <summary>
        /// 文件名称
        /// </summary>
        public string Name { get; set; } = null!;
        /// <summary>
        /// 文件类型（0是简历，1是动态）
        /// </summary>
        public int FileType { get; set; }
        /// <summary>
        /// 与之关联的Id
        /// </summary>
        public int AssociatedId { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime UpdateTime { get; set; }
    }
}