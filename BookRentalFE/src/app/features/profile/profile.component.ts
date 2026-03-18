import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { ModifyProfileRequest } from 'src/app/core/models/user.model';
import { AuthService } from 'src/app/core/services/auth.service';
import { UserService } from 'src/app/core/services/user.service';
import { formatDateString } from 'src/app/shared/helpers/extensions/format-date-string';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.css']
})
export class ProfileComponent implements OnInit {

  isEditable: boolean = false
  selectedFile: File | null = null
  imagePreview: string | ArrayBuffer | null = null
  originalFormValue: any

  constructor(
    public userService: UserService,
    public authService: AuthService,
    private fb: FormBuilder,
    private toastr: ToastrService
  ) { }

  ngOnInit(): void {
    this.getUserAddress()
    this.loadCurrentUser()
  }

  userInfoForm = this.fb.group({
    userName: [{ value: '', disabled: true }, Validators.required],
    phoneNumber: [{ value: '', disabled: true }, Validators.required],
    gender: [{ value: '', disabled: true }],
    dateOfBirth: [{ value: '', disabled: true }, Validators.required],
    fullName: [{ value: '', disabled: true }, Validators.required],
    street: [{ value: '', disabled: true }, Validators.required],
    ward: [{ value: '', disabled: true }, Validators.required],
    district: [{ value: '', disabled: true }, Validators.required],
    city: [{ value: '', disabled: true }, Validators.required],
    postalCode: [{ value: '', disabled: true }, Validators.required]
  })

  getUserAddress() {
    this.userService.getUserAddress().subscribe({
      next: response => {
       if (response) {
        this.userInfoForm && this.userInfoForm.patchValue({
          fullName: response.fullName,
          street: response.street,
          ward: response.ward,
          district: response.district,
          city: response.city,
          postalCode: response.postalCode
        })
        this.setOriginalFromValue()
       } else {
        this.toastr.warning("Chưa cập nhật thông tin địa chỉ")
       }
      }
    })
  }

  loadCurrentUser() {
    const token = localStorage.getItem('access_token')
    this.authService.loadCurrentUser(token).subscribe({
      next: response => {
        if (response) {
          this.userInfoForm && this.userInfoForm.patchValue({
            userName: response.userName,
            phoneNumber: response.phoneNumber,
            gender: response.gender === 'Nam' ? 'male' : 'female',
            dateOfBirth: formatDateString(response.dateOfBirth)
          })
        }
        this.setOriginalFromValue()
      }
    })
  }

  setOriginalFromValue() {
    this.originalFormValue = this.userInfoForm.getRawValue()
  }

  onSubmit() {
    if (this.userInfoForm.valid) {
      const request = this.userInfoForm.value as ModifyProfileRequest
      this.userService.modifyUserInfo(request).subscribe({
        next: response => {
          if (response) {
            this.toastr.success(response.message)
            this.setOriginalFromValue()
            this.isEditable = false
            this.userInfoForm.disable()
          }
        },
        error: (error) => {
          console.log("Có lỗi: ", error)
          this.toastr.error(error.message)
        }
      })
    }
  }

  turnOnEditInfo() {
    this.isEditable = !this.isEditable
    if (this.isEditable) {
      this.userInfoForm.enable()
    } else {
      this.userInfoForm.reset(this.originalFormValue)
      this.userInfoForm.disable()
    }
  }

  onFileSelected(event: any) {
    const file = event.target.files[0]
    if (file) {
      const allowedTypes = ['image/jpeg', 'image/png', 'image/jpeg']
      const maxSizeInMB = 2
      const maxSizeInBytes = maxSizeInMB * 1024 * 1024
      if (!allowedTypes.includes(file.type)) {
        this.toastr.warning('Chỉ chấp nhận định dạng .jpg, .jpeg hoặc .png')
        this.resetSelection()
        return
      }
      if (file.size > maxSizeInBytes) {
        this.toastr.warning(`Dung lượng ảnh không được vượt quá ${maxSizeInMB} MB`)
        this.resetSelection()
        return
      }
      this.selectedFile = file
      const reader = new FileReader()
      reader.onload = (e: any) => {
        this.imagePreview = e.target.result
      }
      reader.readAsDataURL(file)
    }
  }

  uploadImage() {
    if (this.selectedFile) {
      this.userService.uploadUserImage(this.selectedFile).subscribe({
        next: (response) => {
          if (response) {
            this.toastr.success(response.message)
            this.resetSelection()
            this.loadCurrentUser()
          }
        },
        error: (error) => {
          console.log("Có lỗi: ", error)
          this.toastr.error(error.message)
        }
      })
    }
  }

  resetSelection() {
    this.selectedFile = null
    this.imagePreview = null
  }

  cancelUploadImage() {
    this.resetSelection()
  }
}
