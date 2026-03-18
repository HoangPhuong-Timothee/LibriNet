import { Component, OnInit } from '@angular/core';
import { PageEvent } from '@angular/material/paginator';
import { MemberParams } from 'src/app/core/models/params.model';
import { Member } from 'src/app/core/models/user.model';
import { UserService } from 'src/app/core/services/user.service';

@Component({
  selector: 'app-admin-user',
  templateUrl: './admin-user.component.html',
  styleUrls: ['./admin-user.component.css']
})
export class AdminUserComponent implements OnInit {

  searchTerm: string = ''
  memberList: Member[] = []
  adminMemberParams: MemberParams
  totalUsers = 0
  columns = [
    { field: 'id', header: 'ID' },
    { field: ['email', 'imageUrl'], header: 'Email', haveImage: true },
    { field: 'dateOfBirth', header: 'Ngày sinh' },
    { field: 'gender', header: 'Giới tính' },
    { field: 'phoneNumber', header: 'Số điện thoại' },
    { field: 'address', header: 'Địa chỉ' },
    { field: 'roles', header: 'Vai trò' }
  ]

  constructor(
    private userService: UserService
  ) { this.adminMemberParams = this.userService.getMemberParams() }

  ngOnInit(): void {
    this.adminMemberParams.search = ''
    this.getUsersList()
  }

  getUsersList() {
    this.userService.getUsersList().subscribe({
      next: response => {
        if (response.data) {
          this.memberList = response.data
          this.totalUsers = response.count
        }
      }
    })
  }

  onPageChange(event: PageEvent) {
    const params = this.userService.getMemberParams()
    params.pageIndex = event.pageIndex + 1
    params.pageSize = event.pageSize
    this.userService.setMemberParams(params)
    this.adminMemberParams = params
    this.getUsersList()
  }

  onSearch() {
    const params = this.userService.getMemberParams()
    params.search = this.searchTerm
    params.pageIndex = 1
    this.userService.setMemberParams(params)
    this.adminMemberParams = params
    this.getUsersList()
  }

  onReset() {
    if (this.searchTerm) {
      this.searchTerm = ''
    }
    this.adminMemberParams = new MemberParams()
    this.userService.setMemberParams(this.adminMemberParams)
    this.getUsersList()
  }
}
