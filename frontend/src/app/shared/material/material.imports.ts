import { CommonModule } from "@angular/common";
import { MatButtonModule } from "@angular/material/button";
import { MatDialogModule } from "@angular/material/dialog";
import { MatFormFieldModule } from "@angular/material/form-field";
import { MatIconModule } from "@angular/material/icon";
import { MatInputModule } from "@angular/material/input";
import { MatProgressSpinnerModule } from "@angular/material/progress-spinner";
import { MatSnackBarModule } from "@angular/material/snack-bar";
import { MatTableModule } from "@angular/material/table";

export const MATERIAL_IMPORTS = [
        CommonModule,
        MatTableModule,
        MatFormFieldModule,
        MatInputModule,
        MatButtonModule, 
        MatSnackBarModule,
        MatProgressSpinnerModule,
        MatIconModule
];