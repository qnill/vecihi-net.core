using System.ComponentModel.DataAnnotations;
using vecihi.helper.Attributes;
using static vecihi.helper.Const.Enums;

namespace vecihi.domain.Modules
{
    public class AutoCodeAddDto
    {
        /// <summary>
        /// Fixed screen codes
        /// <see cref="Const.ScreenCodes"/>
        /// </summary>
        [Required, MaxLength(5)]
        public string ScreenCode { get; set; }
        /// <summary>
        /// Sample code format = "TC-{0}-RS"
        /// </summary>
        [Required, MaxLength(13)]
        public string CodeFormat { get; set; }
    }

    public class AutoCodeUpdateDto
    {
        /// <summary>
        /// Sample code format = "TC-{0}-RS"
        /// </summary>
        [Required, MaxLength(13)]
        public string CodeFormat { get; set; }
    }

    public class AutoCodeGetDto
    {
        public string ScreenCode { get; set; }
        public string CodeFormat { get; set; }
        public int LastCodeNumber { get; set; }
    }

    public class AutoCodeFilterDto
    {
        [Filter(SearchType.Contains)]
        public string ScreenCode { get; set; }
    }
}
