import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { ProductComponent } from './product/product.component';
import { AttributeComponent } from './attribute/attribute.component';
import { permissionGuard } from '@abp/ng.core';

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
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class CatalogRoutingModule {}
