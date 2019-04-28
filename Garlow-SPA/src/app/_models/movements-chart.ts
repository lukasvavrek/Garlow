import { Movement } from './movement';

export interface MovementsChart {
    sumUntil: number;
    lastMovements: Movement[];
}
