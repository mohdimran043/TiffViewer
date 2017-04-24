using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CustomTiffDemo
{
    public partial class Home : System.Web.UI.Page
    {

        int DefaultThumbHieght = 100;
        int DefaultThumbWidth = 100;
        int DefaultBigHieght = 600;
        int DefaultBigWidth = 600;
        int PagerSize = 8;

        protected void Page_Load(object sender, EventArgs e)
        {
            //Set it so page doesn't cache
            System.Web.UI.HtmlControls.HtmlMeta META = new System.Web.UI.HtmlControls.HtmlMeta();
            META.HttpEquiv = "Pragma";
            META.Content = "no-cache";
            Page.Header.Controls.Add(META);
            Response.Expires = -1;
            Response.CacheControl = "no-cache";

            //Get FilePath
            // FilePath = Request.QueryString["File"];
            FilePath = HttpContext.Current.Server.MapPath("/App_Data") + "\\multipage_tif_example.tif";
            if (FilePath != "")
            {
                //Request.QueryString.Add("FilePath", FilePath);
                //Determine Start/End Pages
                int StartPg = 1;
                if (Request.QueryString["StartPage"] != null)
                    StartPg = System.Convert.ToInt16(Request.QueryString["StartPage"]);
                int BigImgPg = StartPg;
                int EndPg = StartPg + (PagerSize - 1);
                if (EndPg > TotalTIFPgs)
                    EndPg = TotalTIFPgs;

                //Add/configure the thumbnails
                while (StartPg <= EndPg)
                {
                    Label lbl = new Label();
                    if (StartPg % 4 == 0 && StartPg != 0) lbl.Text = "&nbsp;<br />";
                    else lbl.Text = "&nbsp;";
                    Image Img = new Image();
                    Img.BorderStyle = (BorderStyle)Enum.Parse(typeof(BorderStyle), "Solid");
                    Img.BorderWidth = Unit.Parse("1");
                    Img.Attributes.Add("onClick", "ChangePg(" + StartPg.ToString() + ");");
                    Img.Attributes.Add("onmouseover", "this.style.cursor='hand';");
                    Img.ImageUrl = "TiffViewer.aspx?FilePath=" + FilePath + "&Pg=" + (StartPg).ToString() + "&Height=" + DefaultThumbHieght.ToString() + "&Width=" + DefaultThumbWidth;
                    _plcImgsThumbs.Controls.Add(Img);
                    _plcImgsThumbs.Controls.Add(lbl);
                    StartPg++;
                }

                //Bind big img
                Image BigImg = new Image();
                BigImg.BorderStyle = (BorderStyle)Enum.Parse(typeof(BorderStyle), "Solid");
                BigImg.BorderWidth = Unit.Parse("1");
                BigImg.ID = "_imgBig";
                BigImg.ImageUrl = "TiffViewer.aspx?View=1&FilePath=" + FilePath + "&Pg=" + BigImgPg.ToString() + "&Height=" + DefaultBigHieght.ToString() + "&Width=" + DefaultBigWidth.ToString();
                _plcBigImg.Controls.Add(BigImg);

                //Config actions
                _hlRot90.Attributes.Add("OnClick", "document.getElementById('_imgBig').src = 'TiffViewer.aspx?View=1&FilePath=" + FilePath.ToString().Replace(@"\", "%5C") + "&Pg=" + BigImgPg.ToString() + "&Height=" + DefaultBigHieght.ToString() + "&Width=" + DefaultBigWidth.ToString() + "&Rotate=90';");
                _hlRot90.Attributes.Add("onmouseover", "this.style.cursor='hand';");

                _hlRot180.Attributes.Add("OnClick", "document.getElementById('_imgBig').src = 'TiffViewer.aspx?View=1&FilePath=" + FilePath.ToString().Replace(@"\", "%5C") + "&Pg=" + BigImgPg.ToString() + "&Height=" + DefaultBigHieght.ToString() + "&Width=" + DefaultBigWidth.ToString() + "&Rotate=180';");
                _hlRot180.Attributes.Add("onmouseover", "this.style.cursor='hand';");

                _hlRot270.Attributes.Add("OnClick", "document.getElementById('_imgBig').src = 'TiffViewer.aspx?View=1&FilePath=" + FilePath.ToString().Replace(@"\", "%5C") + "&Pg=" + BigImgPg.ToString() + "&Height=" + DefaultBigHieght.ToString() + "&Width=" + DefaultBigWidth.ToString() + "&Rotate=270';");
                _hlRot270.Attributes.Add("onmouseover", "this.style.cursor='hand';");

                _hlBig.Attributes.Add("OnClick", "document.getElementById('_imgBig').src = 'TiffViewer.aspx?View=1&FilePath=" + FilePath.ToString().Replace(@"\", "%5C") + "&Pg=" + BigImgPg.ToString() + "&Height=" + (DefaultBigHieght + 400).ToString() + "&Width=" + (DefaultBigWidth + 400).ToString() + "';");
                _hlBig.Attributes.Add("onmouseover", "this.style.cursor='hand';");

                _hlSmall.Attributes.Add("OnClick", "document.getElementById('_imgBig').src = 'TiffViewer.aspx?View=1&FilePath=" + FilePath.ToString().Replace(@"\", "%5C") + "&Pg=" + BigImgPg.ToString() + "&Height=" + (DefaultBigHieght).ToString() + "&Width=" + (DefaultBigWidth).ToString() + "';");
                _hlSmall.Attributes.Add("onmouseover", "this.style.cursor='hand';");

                //ConfigPagers
                //Config page 1 - whatever
                if ((TotalTIFPgs / PagerSize) >= 1)
                {
                    HyperLink _hl = new HyperLink();
                    Label lbl = new Label(); lbl.Text = "&nbsp;";
                    if (Request.Url.ToString().IndexOf("&StartPage=") >= 0)
                        _hl.NavigateUrl = Request.Url.ToString().Substring(0, Request.Url.ToString().IndexOf("&StartPage=")) + "&StartPage=1";
                    else
                        _hl.NavigateUrl = Request.Url.ToString() + "&StartPage=1";
                    if ((1 + PagerSize) > TotalTIFPgs)
                        _hl.Text = "1-" + TotalTIFPgs;
                    else
                        _hl.Text = "1-" + PagerSize;
                    _plcImgsThumbsPager.Controls.Add(_hl);
                    _plcImgsThumbsPager.Controls.Add(lbl);
                }
                //Config the rest of the page pagers
                for (int i = 1; i <= (TotalTIFPgs / PagerSize); i++)
                {
                    HyperLink _hl = new HyperLink();
                    Label lbl1 = new Label(); lbl1.Text = "&nbsp;";
                    if (Request.Url.ToString().IndexOf("&StartPage=") >= 0)
                        _hl.NavigateUrl = Request.Url.ToString().Substring(0, Request.Url.ToString().IndexOf("&StartPage=")) + "&StartPage=" + ((i * PagerSize) + 1).ToString();
                    else
                        _hl.NavigateUrl = Request.Url.ToString() + "&StartPage=" + ((i * PagerSize) + 1).ToString();
                    if (i == (TotalTIFPgs / PagerSize))
                        _hl.Text = ((i * PagerSize) + 1).ToString() + "-" + TotalTIFPgs;
                    else
                        _hl.Text = ((i * PagerSize) + 1).ToString() + "-" + (((i + 1) * PagerSize)).ToString();
                    _plcImgsThumbsPager.Controls.Add(_hl);
                    _plcImgsThumbsPager.Controls.Add(lbl1);
                }
            }
            else
            {
                Response.Write("Please provde a file path");
            }
        }

        public int TotalTIFPgs
        {
            get
            {
                if (ViewState["TotalTIFPgs"] == null)
                {
                    TIF TheFile = new TIF(FilePath);
                    ViewState["TotalTIFPgs"] = TheFile.PageCount;
                    TheFile.Dispose();
                }
                return System.Convert.ToInt16(ViewState["TotalTIFPgs"]);
            }
            set
            {
                ViewState["TotalTIFPgs"] = value;
            }
        }

        public String FilePath
        {
            get
            {
                if (ViewState["FilePath"] == null)
                {
                    return "";
                }
                return ViewState["FilePath"].ToString();
            }
            set
            {
                ViewState["FilePath"] = value;
            }
        }
    }
}
