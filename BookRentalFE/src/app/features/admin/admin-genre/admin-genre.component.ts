import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { PageEvent } from '@angular/material/paginator';
import { ToastrService } from 'ngx-toastr';
import { Genre } from 'src/app/core/models/genre.model';
import { GenreParams } from 'src/app/core/models/params.model';
import { DialogService } from 'src/app/core/services/dialog.service';
import { GenreService } from 'src/app/core/services/genre.service';
import { AddGenreFormComponent } from './add-genre-form/add-genre-form.component';
import { EditGenreFormComponent } from './edit-genre-form/edit-genre-form.component';
import { ImportGenreFormComponent } from './import-genre-form/import-genre-form.component';

@Component({
  selector: 'app-admin-genre',
  templateUrl: './admin-genre.component.html',
  styleUrls: ['./admin-genre.component.css']
})
export class AdminGenreComponent implements OnInit {

  searchTerm: string = ''
  genreList: Genre[] = []
  adminGenreParams: GenreParams
  totalGenres: number = 0
  columns = [
    { field: 'id', header: 'Mã thể loại' },
    { field: 'name', header: 'Thể loại' },
    {
      field: 'createInfo',
      header: 'Tạo bởi',
      class: () => {
        return 'fst-italic'
      }
    },
    {
      field: 'updateInfo',
      header: 'Cập nhật bởi',
      class: () => {
        return 'fst-italic'
      }
    }
  ]
  actions = [
    {
      label: 'Cập nhật',
      icon: 'edit',
      tooltip: 'Chỉnh sửa thể loại',
      action: (row: any) => {
        this.openUpdateGenreDialog(row)
      }
    },
    {
      label: 'Xóa',
      icon: 'delete',
      tooltip: 'Xóa thể loại',
      action: (row: any) => {
        this.openDeleteGenreDialog(row)
      }
    }
  ]

  constructor(
    private genreService: GenreService,
    private dialog: MatDialog,
    private dialogService: DialogService,
    private toastr: ToastrService
  )
  {
    this.adminGenreParams = genreService.getGenreParams()
  }

  ngOnInit(): void {
    this.adminGenreParams.search = ''
    this.getAllGenresForAdmin()
  }

  getAllGenresForAdmin(): void {
    this.genreService.getGenresForAdmin().subscribe({
      next: response => {
        this.genreList = response.data
        this.totalGenres = response.count
      },
      error: error => {
        console.log(error)
      }
    })
  }

  openAddNewGenreDialog(): void {
    const dialog = this.dialog.open(AddGenreFormComponent, {
        minWidth: '500px',
        data: {
            title: 'Thêm thể loại mới'
        }
    })
    dialog.afterClosed().subscribe({
      next: (result) => {
        if (result && result.success) {
          const params = this.genreService.getGenreParams()
          params.pageIndex = 1
          this.genreService.setGenreParams(params)
          this.adminGenreParams = params
          this.getAllGenresForAdmin()
        }
      }
    })
  }

  openImportGenreDialog(): void {
    const dialog = this.dialog.open(ImportGenreFormComponent, {
        minWidth: '200px',
        data: {
            title: 'Nhập dữ liệu thể loại'
        }
    })
    dialog.afterClosed().subscribe({
      next: (result) => {
        if (result && result.importSuccess) {
          const params = this.genreService.getGenreParams()
          params.pageIndex = 1
          this.genreService.setGenreParams(params)
          this.adminGenreParams = params
          this.getAllGenresForAdmin()
        }
      }
    })
  }

  openUpdateGenreDialog(genre: Genre) {
    const dialog = this.dialog.open(EditGenreFormComponent, {
      minWidth: '500px',
      data: {
        title: 'Chỉnh sửa thể loại',
        genre
      }
    })
    dialog.afterClosed().subscribe({
      next: result => {
        if (result) {
          if (result && result.success) {
            const params = this.genreService.getGenreParams()
            params.pageIndex = 1
            this.genreService.setGenreParams(params)
            this.adminGenreParams = params
            this.getAllGenresForAdmin()
          }
        }
      }
    })
  }

  async openDeleteGenreDialog(genre: Genre): Promise<void> {
    const confirmed = await this.dialogService.confirmDialog(
      'XÁC NHẬN XÓA',
      `Bạn chắc chắn muốn xóa thể loại "${genre.name}"?`
    )
    if (confirmed) {
      this.genreService.deleteGenre(genre.id).subscribe({
        next: async () => {
          this.genreList = this.genreList.filter(g => g.id !== genre.id)
          this.toastr.success(`Xóa thể loại "${genre.name}" thành công`)
        },
        error: error => {
          console.log("Có lỗi xảy ra: ", error)
        }
      })
    }
  }

  onPageChange(event: PageEvent) {
    const params = this.genreService.getGenreParams()
    params.pageIndex = event.pageIndex + 1
    params.pageSize = event.pageSize
    this.genreService.setGenreParams(params)
    this.adminGenreParams = params
    this.getAllGenresForAdmin()
  }

  onSearch() {
    const params = this.genreService.getGenreParams()
    params.search = this.searchTerm
    params.pageIndex = 1
    this.genreService.setGenreParams(params)
    this.adminGenreParams = params
    this.getAllGenresForAdmin()
  }

  onReset() {
    if (this.searchTerm) {
      this.searchTerm = ''
    }
    this.adminGenreParams = new GenreParams()
    this.genreService.setGenreParams(this.adminGenreParams)
    this.getAllGenresForAdmin()
  }

}
