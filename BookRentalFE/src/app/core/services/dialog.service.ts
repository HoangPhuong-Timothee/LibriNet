import { Injectable } from "@angular/core";
import { MatDialog } from "@angular/material/dialog";
import { firstValueFrom } from "rxjs";
import { ConfirmationDialogComponent } from "src/app/shared/components/confirmation-dialog/confirmation-dialog.component";

@Injectable({
    providedIn: "root"
})

export class DialogService {

  constructor(private dialog: MatDialog) { }

  confirmDialog(message: string, title: string) {
    const dialogRef = this.dialog.open(ConfirmationDialogComponent, {
        data: {
          message,
          title
        },
        width: "500px"
    })
    return firstValueFrom(dialogRef.afterClosed())
  }

}
