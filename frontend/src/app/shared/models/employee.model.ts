export interface Employee {
    id: string;
    firstName: string;
    lastName: string;
    email: string;
    departmentId?: string | null;
    department?: string | null;
}