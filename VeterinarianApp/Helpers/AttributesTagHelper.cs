using Microsoft.AspNetCore.Razor.TagHelpers;

namespace VeterinarianApp.Helpers
{
    [HtmlTargetElement("input", Attributes = "is-edit-mode")]
    public class AttributesTagHelper : TagHelper
    {
        [HtmlAttributeName("is-edit-mode")]
        public bool IsEditMode { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (IsEditMode)
            {
                var existingAttributes = output.Attributes.ToList();
                output.Attributes.Clear();

                foreach (var attribute in existingAttributes)
                {
                    if (attribute.Name == "readonly")
                    {
                        continue;
                    }

                    output.Attributes.Add(attribute.Name, attribute.Value);
                }

                output.Attributes.Add("readonly", "readonly");
            }
        }
    }

}
