import type { EntityDto } from '@abp/ng.core';

export interface CreateUpdateProductCategoryDto {
  name?: string;
  code?: string;
  slug?: string;
  sortOrder: number;
  visibility: boolean;
  isActive: boolean;
  seoMetaDescription?: string;
}

export interface ProductCategoryDto {
  name?: string;
  code?: string;
  slug?: string;
  sortOrder: number;
  visibility: boolean;
  isActive: boolean;
  seoMetaDescription?: string;
  id?: string;
}

export interface ProductCategoryInListDto extends EntityDto<string> {
  name?: string;
  code?: string;
  sortOrder: number;
  visibility: boolean;
  isActive: boolean;
  id?: string;
}
