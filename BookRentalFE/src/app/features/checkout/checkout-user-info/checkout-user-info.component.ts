import { Component, Input } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { ModifyAddressRequest } from 'src/app/core/models/address.model';
import { UserService } from 'src/app/core/services/user.service';

@Component({
  selector: 'app-checkout-user-info',
  templateUrl: './checkout-user-info.component.html',
  styleUrls: ['./checkout-user-info.component.css']
})
export class CheckoutUserInfoComponent {

  @Input() checkoutForm?: FormGroup

  constructor(
    private userService: UserService,
    private toastr: ToastrService
  ) { }

  saveUserInfo() {
    const userInfo = this.checkoutForm?.get('userInfoForm')?.value as ModifyAddressRequest
    this.userService.modifyUserAddress(userInfo).subscribe({
      next: () => {
        this.toastr.success("Đã lưu thành thông tin mặc định thành công")
        this.checkoutForm?.get("userInfoForm")?.reset(userInfo)
      }
    })
  }

}
