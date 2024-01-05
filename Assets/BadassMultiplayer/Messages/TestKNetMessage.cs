using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestKNetMessage : KNetworkObjectMessage
{
    public TestKNetMessage():base() { }
    public TestKNetMessage(KNetworkObject obj) : base(obj) { }
    
}
