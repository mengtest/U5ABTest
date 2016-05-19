using UnityEngine;
using System.Collections;

[ProtoBuf.ProtoContract]
public class Person
{
    [ProtoBuf.ProtoMember(1)]
    public int Id { get; set; }
    [ProtoBuf.ProtoMember(2)]
    public string Name { get; set; }
    [ProtoBuf.ProtoMember(3)]
    public Address Address { get; set; }
}

[ProtoBuf.ProtoContract]
public class Address
{
    [ProtoBuf.ProtoMember(1)]
    public string Line1 { get; set; }
    [ProtoBuf.ProtoMember(2)]
    public string Line2 { get; set; }
}