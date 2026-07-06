import { Component, inject } from "@angular/core";
import { MatDialogRef } from "@angular/material/dialog";

@Component({
    standalone: true,
    template: `
    <h2>Delete Employee?</h2>

    <p>This action cannot be undone.</p>

    <button (click)="confirm()">Delete</button>
    <button (click)="cancel()">Cancel</button>
  `
})
export class ConfirmDeleteDialog {

    private dialogRef = inject(MatDialogRef);

    confirm() {
        this.dialogRef.close(true);
    }

    cancel() {
        this.dialogRef.close(false);
    }
}