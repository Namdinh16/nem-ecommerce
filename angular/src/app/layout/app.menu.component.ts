import { OnInit } from '@angular/core';
import { Component } from '@angular/core';
import { LayoutService } from './service/app.layout.service';

@Component({
    selector: 'app-menu',
    templateUrl: './app.menu.component.html'
})
export class AppMenuComponent implements OnInit {

    model: any[] = [];

    constructor(public layoutService: LayoutService) { }

    ngOnInit() {
        this.model = [
            {
                label: 'Trang chủ',
                items: [
                    { label: 'Dashboard', icon: 'pi pi-fw pi-home', routerLink: ['/'] }
                ]
            },

            {
                label: 'Sản phẩm',
                items: [
                    { label: 'Danh sách sản phẩm', icon: 'pi pi-fw pi-list', routerLink: ['/product'] },
                    { label: 'Danh sách thuộc tính', icon: 'pi pi-fw pi-list', routerLink: ['/attribute'] },
                ]
            },

            {
                label: 'Hệ thống',
                items: [
                    { label: 'Danh sách quyền', icon: 'pi pi-fw pi-users', routerLink: ['/role'] },
                    { label: 'Danh sách người dùng', icon: 'pi pi-fw pi-user', routerLink: ['/user'] },
                ]
            },
        ];
    }
}
