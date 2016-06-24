namespace Constellation.Web
{
	using HtmlAgilityPack;
	using System.Linq;
	using System.Text;


	public class GoogleAmpConverter
	{
		private readonly string source;

		public GoogleAmpConverter(string source)
		{
			this.source = source;
		}

		public static string Convert(string source)
		{
			var converter = new GoogleAmpConverter(source);

			return converter.Convert();
		}

		public string Convert()
		{
			var result = ReplaceIframeWithLink(source);

			result = UpdateAmpImages(result);

			return result;
		}

		private string ReplaceIframeWithLink(string current)
		{
			var doc = GetHtmlDocument(current);
			var elements = doc.DocumentNode.Descendants("iframe");
			foreach (var htmlNode in elements)
			{
				if (htmlNode.Attributes["src"] == null) continue;

				var link = htmlNode.Attributes["src"].Value;
				var paragraph = doc.CreateElement("p");

				var text = link; // TODO: This might need to be expanded in the future

				var anchor = doc.CreateElement("a");
				anchor.InnerHtml = text;
				anchor.Attributes.Add("href", link);
				anchor.Attributes.Add("title", text);
				paragraph.InnerHtml = anchor.OuterHtml;

				var original = htmlNode.OuterHtml;
				var replacement = paragraph.OuterHtml;

				current = current.Replace(original, replacement);
			}

			return current;
		}

		private string UpdateAmpImages(string current)
		{
			// Use HtmlAgilityPack (install-package HtmlAgilityPack)
			var doc = GetHtmlDocument(current);
			var imageList = doc.DocumentNode.Descendants("img");

			const string ampImage = "amp-img";

			if (!imageList.Any()) return current;

			if (!HtmlNode.ElementsFlags.ContainsKey("amp-img"))
			{
				HtmlNode.ElementsFlags.Add("amp-img", HtmlElementFlag.Closed);
			}

			foreach (var imgTag in imageList)
			{
				var original = imgTag.OuterHtml;
				var replacement = imgTag.Clone();
				replacement.Name = ampImage;
				replacement.Attributes.Remove("caption");
				current = current.Replace(original, replacement.OuterHtml);
			}

			return current;
		}

		private HtmlDocument GetHtmlDocument(string htmlContent)
		{
			var doc = new HtmlDocument
			{
				OptionOutputAsXml = true,
				OptionDefaultStreamEncoding = Encoding.UTF8
			};
			doc.LoadHtml(htmlContent);

			return doc;
		}
	}
}
