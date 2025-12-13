export type Department = {
  id: string;
  name: string;
  depth: number;
  identifier: string;
  parentId: string | null;
  path: string;
  childCount: number;
  isActive: boolean;
  createdAt: Date;
  updatedAt: Date;
  deletedAt: boolean;
  children: Department[];
};
