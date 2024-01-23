using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INOVI.DTO;

public class GetQueryDTO
{
    public int QueryId { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string ShortDescription { get; set; }
    public string CurrentStatus { get; set; }
    public string ColorCode { get; set; }
    public List<byte[]> Attachmentbytes { get; set; }
    public string Remarks { get; set; }

}

public class AddQueryDTO
{
    public string Title { get; set; }
    public string Description { get; set; }
    public List<AttachmentLinkList> AttachmentIds { get; set; } = new List<AttachmentLinkList>();
    public List<string> Attachments { get; set; }
    public int UserID { get; set; }

}
public class AttachmentLinkList
{
    public int AttachmentLinkId { get; set; }
    public string FileName { get; set; }
    public string AttachmentLink { get; set; }
    public string Path { get; set; }
}

public class UpdateStatusDTO
{
    public int QueryID { get; set; }
    public int StatusID { get; set; }
    public int UserID { get; set; }
    public string Remarks { get; set; }
}

