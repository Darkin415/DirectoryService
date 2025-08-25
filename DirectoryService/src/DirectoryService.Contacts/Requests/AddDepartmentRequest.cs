﻿namespace DirectoryService.Contacts.Requests;

public record AddDepartmentRequest(string Name, string Identifier, Guid? ParentId,  List<Guid> Locations);
