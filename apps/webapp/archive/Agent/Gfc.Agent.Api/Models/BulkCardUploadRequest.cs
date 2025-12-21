using System.Collections.Generic;
using Gfc.ControllerClient.Models;

namespace Gfc.Agent.Api.Models;

public sealed class BulkCardUploadRequest
{
    public IList<CardPrivilegeModel> Cards { get; set; } = new List<CardPrivilegeModel>();
}

