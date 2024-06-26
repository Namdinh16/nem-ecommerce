import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { ProductComponent } from './product/product.component';
import { AttributeComponent } from './attribute/attribute.component';
import { permissionGuard } from '@abp/ng.core';
import { CategoryComponent } from './category/category.component';
import { ManufactureComponent } from './manufacture/manufacture.component';

const routes: Routes = [
  { 
    path: 'product', 
    component: ProductComponent, 
    canActivate: [permissionGuard], 
    data: {
      requirePolicy: 'NemEcommerceAdminCatalog.Product',
    },  
  },
  { 
    path: 'attribute', 
    component: AttributeComponent, 
    canActivate: [permissionGuard], 
    data: {
      requirePolicy: 'NemEcommerceAdminCatalog.Attribute',
    },   
  },
  { 
    path: 'category', 
    component: CategoryComponent, 
    canActivate: [permissionGuard], 
    data: {
      requirePolicy: 'NemEcommerceAdminCatalog.ProductCategory',
    },   
  },
  { 
    path: 'manufacture', 
    component: ManufactureComponent, 
    canActivate: [permissionGuard], 
    data: {
      requirePolicy: 'NemEcommerceAdminCatalog.Manufacturer',
    },   
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class CatalogRoutingModule {}
